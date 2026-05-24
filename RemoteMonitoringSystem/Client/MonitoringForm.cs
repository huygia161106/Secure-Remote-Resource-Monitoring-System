using System;
using System.Diagnostics;
using System.Management;
using System.Net;
using System.IO;
using System.Net.NetworkInformation;
using System.Net.Security;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using LibreHardwareMonitor.Hardware;
using Newtonsoft.Json;

namespace Client
{
    /// <summary>
    /// Module Client Agent (Tác nhân trạm): Chạy ngầm trên máy khách (Endpoint).
    /// Chức năng chính:
    /// - Thu thập dữ liệu đo lường phần cứng (Telemetry).
    /// - Giám sát hành vi người dùng (Active Windows) và cảnh báo các tiến trình nhạy cảm.
    /// - Giao tiếp với máy chủ trung tâm qua đường hầm mã hóa mTLS.
    /// - Lắng nghe và thực thi các chỉ thị điều khiển (ví dụ: Kill Process) từ xa.
    /// </summary>
    public partial class MonitoringForm : Form
    {
        #region --- BIẾN TOÀN CỤC & TÀI NGUYÊN HỆ THỐNG ---

        // Tài nguyên giám sát phần cứng
        private PerformanceCounter _cpuCounter;
        private Computer _computer;

        // Trạng thái mạng
        private long _prevBytesReceived = 0;
        private long _prevBytesSent = 0;

        // Giám sát hành vi
        private string _lastActiveWindow = "";
        private CancellationTokenSource _activityCts;

        // Thông tin định danh Agent
        private string _currentShareCode = "";
        private readonly string _machineName = Environment.MachineName;
        private string _ipAddress = "127.0.0.1";
        private readonly string _currentUsername;

        // Luồng giao tiếp mạng (mTLS)
        private TcpClient _currentClient;
        private SslStream _currentSslStream;
        private StreamWriter _currentWriter;
        private StreamReader _currentReader;

        // Kiểm soát đồng bộ hóa (Thread Synchronization)
        private bool _isSending = false;
        private readonly SemaphoreSlim _networkLock = new SemaphoreSlim(1, 1);

        // Danh sách các tiến trình lõi của hệ điều hành (Không cho phép can thiệp/tắt)
        private readonly string[] _protectedProcesses = {
            "svchost", "explorer", "csrss", "wininit", "smss", "services", "lsass", "system"
        };

        #endregion

        #region --- KHỞI TẠO & VẬN HÀNH ---

        public MonitoringForm(string username)
        {
            InitializeComponent();
            _currentUsername = username;
            _ipAddress = GetLocalIPAddress();

            InitializeCounters();
        }

        /// <summary>
        /// Khởi tạo các bộ đếm hiệu năng và cảm biến nhiệt độ.
        /// Yêu cầu đặc quyền Administrator để truy cập driver cấp thấp (Ring 0) thông qua LibreHardwareMonitor.
        /// </summary>
        private void InitializeCounters()
        {
            try
            {
                _cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
                _cpuCounter.NextValue(); // Lần gọi đầu tiên luôn trả về 0, cần gọi trước để lấy mẫu

                _computer = new Computer
                {
                    IsCpuEnabled = true,
                    IsGpuEnabled = true,
                    IsMotherboardEnabled = true,
                    IsStorageEnabled = true
                };
                _computer.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể nạp driver cảm biến. Vui lòng cấp quyền Administrator:\n{ex.Message}",
                                "Cảnh báo quyền truy cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async void btnStart_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = false;
            lblStatus.Text = "Trạng thái: Đang thiết lập đường hầm bảo mật mTLS...";
            lblStatus.ForeColor = System.Drawing.Color.DarkOrange;

            bool isConnected = await ConnectToServerAsync();

            if (isConnected)
            {
                btnStop.Enabled = true;
                lblStatus.Text = $"Trạng thái: Đang truyền phát Telemetry | ID: {_currentShareCode}";
                lblStatus.ForeColor = System.Drawing.Color.Green;

                timerPush.Start(); // Bắt đầu chu trình đẩy dữ liệu định kỳ
            }
            else
            {
                btnStart.Enabled = true;
            }
        }

        #endregion

        #region --- KẾT NỐI MẠNG & BẢO MẬT (mTLS) ---

        /// <summary>
        /// Thiết lập kết nối TCP và thực hiện bắt tay Mutual TLS (mTLS) với máy chủ.
        /// Quá trình bao gồm: Kết nối -> Xác thực chứng chỉ -> Đăng ký định danh Agent.
        /// </summary>
        private async Task<bool> ConnectToServerAsync()
        {
            try
            {
                _currentClient = new TcpClient { NoDelay = true };
                var connectTask = _currentClient.ConnectAsync("127.0.0.1", 8888);

                // Timeout kết nối mạng sau 2 giây
                if (await Task.WhenAny(connectTask, Task.Delay(2000)).ConfigureAwait(false) != connectTask)
                    throw new TimeoutException("Máy chủ trung tâm không phản hồi.");

                await connectTask.ConfigureAwait(false);

                // Khởi tạo luồng SSL/TLS
                _currentSslStream = new SslStream(_currentClient.GetStream(), false, ValidateServerCertificate);

                // Nạp chứng chỉ Client để Server xác thực (Mutual Authentication)
                X509Certificate2 clientCertificate = new X509Certificate2("ClientCertECC.pfx", "NT106.Q23");
                X509CertificateCollection clientCerts = new X509CertificateCollection(new[] { clientCertificate });

                await _currentSslStream.AuthenticateAsClientAsync("RemoteMonitorServer", clientCerts, SslProtocols.Tls12, false).ConfigureAwait(false);

                _currentWriter = new StreamWriter(_currentSslStream, Encoding.UTF8) { AutoFlush = true };
                _currentReader = new StreamReader(_currentSslStream, Encoding.UTF8);

                // Gửi payload đăng ký Agent
                var regData = new { Type = "REGISTER_AGENT", MachineName = _machineName, IP = _ipAddress, Username = _currentUsername };
                await _currentWriter.WriteLineAsync(JsonConvert.SerializeObject(regData));

                // Lắng nghe cấp phát từ Server
                string response = await _currentReader.ReadLineAsync();
                if (response != null && response.StartsWith("REGISTER_OK|"))
                {
                    string[] parts = response.Split('|');
                    _currentShareCode = parts[1];
                    string sessionPass = parts[2];

                    this.Invoke((Action)(() => {
                        lblShareCode.Text = $"ID: {_currentShareCode}   |   PASS: {sessionPass}";
                    }));
                }
                else throw new Exception("Máy chủ từ chối cấp phát mã phiên (Session ID).");

                // Khởi chạy các luồng nghiệp vụ chạy ngầm
                StartActivityTracker();
                _ = Task.Run(() => ListenForCommands());

                return true;
            }
            catch (Exception ex)
            {
                this.Invoke((Action)(() => {
                    lblStatus.Text = $"Lỗi giao thức mạng: {ex.Message}";
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                }));
                return false;
            }
        }

        #endregion

        #region --- THU THẬP TELEMETRY (HIỆU NĂNG & NHIỆT ĐỘ) ---

        private async void timerPush_Tick(object sender, EventArgs e)
        {
            if (_isSending) return;
            _isSending = true;

            try
            {
                // Kiểm tra tính toàn vẹn của kết nối
                if (_currentSslStream == null || _currentClient == null || !_currentClient.Connected)
                {
                    btnStop_Click(null, null);
                    return;
                }

                // 1. Thu thập tải CPU, RAM, Disk
                float cpuVal = await Task.Run(() => _cpuCounter.NextValue()).ConfigureAwait(false);
                string cpu = Math.Round(cpuVal, 1).ToString();

                Microsoft.VisualBasic.Devices.ComputerInfo ci = new Microsoft.VisualBasic.Devices.ComputerInfo();
                double totalRamMB = ci.TotalPhysicalMemory / (1024.0 * 1024.0);
                double availableRamMB = ci.AvailablePhysicalMemory / (1024.0 * 1024.0);
                double usedRamMB = totalRamMB - availableRamMB;
                string ramUsage = Math.Round((usedRamMB / totalRamMB) * 100, 1).ToString();

                DriveInfo drive = new DriveInfo("C");
                double diskFreePercent = (double)drive.AvailableFreeSpace / drive.TotalSize * 100;
                string disk = Math.Round(100 - diskFreePercent, 1).ToString();

                // 2. Thu thập băng thông mạng
                long currentReceived = 0, currentSent = 0;
                foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (ni.OperationalStatus == OperationalStatus.Up)
                    {
                        currentReceived += ni.GetIPv4Statistics().BytesReceived;
                        currentSent += ni.GetIPv4Statistics().BytesSent;
                    }
                }
                double downloadSpeedKBps = (_prevBytesReceived != 0) ? (currentReceived - _prevBytesReceived) / 2.0 / 1024.0 : 0;
                double uploadSpeedKBps = (_prevBytesSent != 0) ? (currentSent - _prevBytesSent) / 2.0 / 1024.0 : 0;
                _prevBytesReceived = currentReceived;
                _prevBytesSent = currentSent;

                // 3. Thu thập thông số nhiệt độ phần cứng (LibreHardwareMonitor)
                string cpuTemp = "0", gpuTemp = "0", hddTemp = "0", boardTemp = "0";

                if (_computer != null)
                {
                    _computer.Accept(new UpdateVisitor());

                    void CollectTemps(IHardware hardware)
                    {
                        foreach (ISensor sensor in hardware.Sensors)
                        {
                            if (sensor.SensorType != SensorType.Temperature || !sensor.Value.HasValue) continue;

                            float val = (float)Math.Round(sensor.Value.Value, 1);
                            if (val <= 0 || val > 150) continue; // Lọc nhiễu cảm biến

                            switch (hardware.HardwareType)
                            {
                                case HardwareType.Cpu:
                                    if (sensor.Name.IndexOf("Package", StringComparison.OrdinalIgnoreCase) >= 0)
                                        cpuTemp = val.ToString("F1");
                                    else if (cpuTemp == "0")
                                        cpuTemp = val.ToString("F1");
                                    break;

                                case HardwareType.GpuNvidia:
                                case HardwareType.GpuAmd:
                                case HardwareType.GpuIntel:
                                    if (sensor.Name.IndexOf("GPU Core", StringComparison.OrdinalIgnoreCase) >= 0)
                                        gpuTemp = val.ToString("F1");
                                    else if (gpuTemp == "0")
                                        gpuTemp = val.ToString("F1");
                                    break;

                                case HardwareType.Storage:
                                    if (sensor.Name.IndexOf("Assembly", StringComparison.OrdinalIgnoreCase) >= 0 || sensor.Name == "Temperature")
                                        hddTemp = val.ToString("F1");
                                    else if (hddTemp == "0")
                                        hddTemp = val.ToString("F1");
                                    break;

                                case HardwareType.Motherboard:
                                case HardwareType.SuperIO:
                                    if (val < 100) boardTemp = val.ToString("F1");
                                    break;
                            }
                        }

                        // Đệ quy để lấy dữ liệu từ các chip điều khiển con (SubHardware)
                        foreach (IHardware sub in hardware.SubHardware)
                            CollectTemps(sub);
                    }

                    foreach (IHardware hardware in _computer.Hardware)
                        CollectTemps(hardware);
                }

                // 4. Cơ chế dự phòng (Fallback) lấy nhiệt độ từ WMI/ACPI nếu thư viện LHM bị hạn chế cấp quyền
                bool needCpuFallback = (cpuTemp == "0");
                bool needBoardFallback = (boardTemp == "0");

                if (needCpuFallback || needBoardFallback)
                {
                    try
                    {
                        using (var searcher = new ManagementObjectSearcher(@"root\WMI", "SELECT * FROM MSAcpi_ThermalZoneTemperature"))
                        {
                            foreach (ManagementObject obj in searcher.Get())
                            {
                                // WMI trả về giá trị đơn vị deciKelvin. Chuyển đổi sang Celsius: C = (K / 10) - 273.15
                                double deciKelvin = Convert.ToDouble(obj["CurrentTemperature"]);
                                double celsius = (deciKelvin / 10.0) - 273.15;

                                if (celsius <= 0 || celsius > 120) continue;

                                string formatted = Math.Round(celsius, 1).ToString("F1");

                                if (needBoardFallback)
                                {
                                    boardTemp = formatted;
                                    needBoardFallback = false;
                                }

                                if (needCpuFallback)
                                {
                                    cpuTemp = formatted;
                                    needCpuFallback = false;
                                }

                                if (!needBoardFallback && !needCpuFallback) break;
                            }
                        }
                    }
                    catch { /* Fallback thất bại, hệ thống tự động hiển thị N/A trên UI */ }
                }

                // 5. Thu thập danh sách tiến trình (Process Inventory)
                StringBuilder appBuilder = new StringBuilder();
                foreach (Process p in Process.GetProcesses())
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(p.MainWindowTitle))
                        {
                            long memoryMB = p.WorkingSet64 / (1024 * 1024);
                            appBuilder.Append($"{p.ProcessName}.exe|{p.MainWindowTitle}|{memoryMB} MB;");
                        }
                    }
                    catch { /* Bỏ qua các process bị khóa quyền truy cập */ }
                }
                string appList = appBuilder.ToString().TrimEnd(';');
                if (string.IsNullOrEmpty(appList)) appList = "NONE";

                // 6. Đóng gói Payload và truyền tải
                var resourceData = new
                {
                    Type = "PUSH_RESOURCE",
                    ShareCode = _currentShareCode,
                    Cpu = cpu,
                    Ram = ramUsage,
                    Disk = disk,
                    NetDown = Math.Round(downloadSpeedKBps, 1).ToString(),
                    NetUp = Math.Round(uploadSpeedKBps, 1).ToString(),
                    MachineName = _machineName,
                    IP = _ipAddress,
                    AppList = appList,
                    CpuTemp = cpuTemp,
                    GpuTemp = gpuTemp,
                    HddTemp = hddTemp,
                    BoardTemp = boardTemp
                };

                await _networkLock.WaitAsync();
                try
                {
                    if (_currentWriter != null && _currentClient.Connected)
                    {
                        await _currentWriter.WriteLineAsync(JsonConvert.SerializeObject(resourceData));

                        // Cảnh báo thời gian thực (Real-time Alerts) nếu tài nguyên vượt ngưỡng
                        if (cpuVal >= 80)
                        {
                            var cpuAlert = new { Type = "PUSH_REALTIME_LOG", ShareCode = _currentShareCode, LogType = "Warning", Source = "System Monitor", Time = DateTime.Now.ToString("HH:mm:ss"), Message = $"[BẢO MẬT HỆ THỐNG] Ngưỡng CPU vượt mức an toàn ({cpu}%)!" };
                            await _currentWriter.WriteLineAsync(JsonConvert.SerializeObject(cpuAlert));
                        }

                        double ramPercent = (usedRamMB / totalRamMB) * 100;
                        if (ramPercent >= 80)
                        {
                            var ramAlert = new { Type = "PUSH_REALTIME_LOG", ShareCode = _currentShareCode, LogType = "Warning", Source = "System Monitor", Time = DateTime.Now.ToString("HH:mm:ss"), Message = $"[BẢO MẬT HỆ THỐNG] Ngưỡng RAM vượt mức an toàn ({Math.Round(ramPercent, 1)}%)!" };
                            await _currentWriter.WriteLineAsync(JsonConvert.SerializeObject(ramAlert));
                        }
                    }
                }
                finally { _networkLock.Release(); }
            }
            catch
            {
                this.Invoke((Action)(() => {
                    btnStop_Click(null, null);
                    lblStatus.Text = "Lỗi đồng bộ: Kết nối tới máy chủ trung tâm bị gián đoạn.";
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                }));
            }
            finally { _isSending = false; }
        }

        #endregion

        #region --- GIÁM SÁT HÀNH VI (WIN32 API) & PHÁT HIỆN RỦI RO ---

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, out uint ProcessId);

        /// <summary>
        /// Khởi chạy Background Task giám sát sự thay đổi cửa sổ hoạt động của người dùng (Active Windows).
        /// </summary>
        private void StartActivityTracker()
        {
            _activityCts = new CancellationTokenSource();
            Task.Run(async () => {
                while (!_activityCts.Token.IsCancellationRequested)
                {
                    CheckActiveWindow();
                    await Task.Delay(1000);
                }
            });
        }

        /// <summary>
        /// Sử dụng thư viện Native User32.dll để đọc tiến trình đang Focus.
        /// Sinh cảnh báo vi phạm bảo mật nếu phát hiện người dùng mở các công cụ dòng lệnh hoặc quản trị hệ thống.
        /// </summary>
        private async void CheckActiveWindow()
        {
            try
            {
                IntPtr handle = GetForegroundWindow();
                if (handle == IntPtr.Zero) return;

                StringBuilder sb = new StringBuilder(256);
                GetWindowText(handle, sb, 256);
                string windowTitle = sb.ToString().Trim();

                if (string.IsNullOrEmpty(windowTitle)) return;

                GetWindowThreadProcessId(handle, out uint pid);
                Process p = Process.GetProcessById((int)pid);
                string processName = p.ProcessName.ToLower();

                string currentActivity = $"{processName}|{windowTitle}";

                if (currentActivity != _lastActiveWindow)
                {
                    _lastActiveWindow = currentActivity;
                    string logType = "Info";

                    // Đưa ra cảnh báo hệ thống khi phát hiện các ứng dụng quản trị nhạy cảm
                    if (processName == "cmd" || processName == "powershell" || processName == "windowsterminal" ||
                        processName == "regedit" || processName == "taskmgr" || processName == "mmc")
                    {
                        logType = "Error";
                    }

                    var logPacket = new
                    {
                        Type = "PUSH_REALTIME_LOG",
                        ShareCode = _currentShareCode,
                        LogType = logType,
                        Source = processName + ".exe",
                        Time = DateTime.Now.ToString("HH:mm:ss"),
                        Message = logType == "Error"
                            ? $"[CẢNH BÁO AN NINH] Phát hiện sử dụng công cụ quản trị hệ thống: {windowTitle}!"
                            : $"[GIÁM SÁT] Phiên thao tác chuyển sang: {windowTitle}"
                    };

                    await _networkLock.WaitAsync();
                    try
                    {
                        if (_currentWriter != null && _currentClient.Connected)
                        {
                            await _currentWriter.WriteLineAsync(JsonConvert.SerializeObject(logPacket));
                        }
                    }
                    catch { }
                    finally { _networkLock.Release(); }
                }
            }
            catch { /* Im lặng bỏ qua lỗi liên kết Win32 API nếu cửa sổ bị đóng quá nhanh */ }
        }

        #endregion

        #region --- THỰC THI LỆNH ĐIỀU KHIỂN TỪ XA (INCIDENT RESPONSE) ---

        /// <summary>
        /// Lắng nghe luồng dữ liệu liên tục từ Server để thực thi các chỉ thị theo thời gian thực.
        /// </summary>
        private async Task ListenForCommands()
        {
            try
            {
                string cmdJson;
                while ((cmdJson = await _currentReader.ReadLineAsync()) != null)
                {
                    dynamic cmd = JsonConvert.DeserializeObject(cmdJson);

                    if (cmd.Type == "KILL_PROCESS")
                    {
                        string pName = ((string)cmd.ProcessName).Replace(".exe", "").ToLower();

                        // Cơ chế bảo vệ Self-defense: Chặn mọi nỗ lực can thiệp vào tiến trình lõi hệ thống
                        bool isProtected = false;
                        foreach (string proc in _protectedProcesses)
                        {
                            if (pName == proc) { isProtected = true; break; }
                        }
                        if (isProtected) continue;

                        Process[] processes = Process.GetProcessesByName(pName);
                        foreach (var process in processes)
                        {
                            try { process.Kill(); } catch { /* Bỏ qua nếu không đủ quyền kill */ }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.Invoke((Action)(() => {
                    btnStop_Click(null, null);
                    lblStatus.Text = $"Luồng lệnh điều khiển bị gián đoạn: {ex.Message}";
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                }));
            }
        }

        #endregion

        #region --- PHƯƠNG THỨC TIỆN ÍCH & VÒNG ĐỜI FORM ---

        private void btnStop_Click(object sender, EventArgs e)
        {
            timerPush.Stop();
            _activityCts?.Cancel();

            _currentWriter?.Close();
            _currentReader?.Close();
            _currentSslStream?.Close();
            _currentClient?.Close();

            btnStart.Enabled = true;
            btnStop.Enabled = false;

            lblStatus.Text = "Trạng thái: Đã ngắt kết nối.";
            lblStatus.ForeColor = System.Drawing.Color.Red;
        }

        /// <summary>
        /// Kiểm chứng tính hợp lệ của chứng chỉ số SSL/TLS (Certificate Validation).
        /// </summary>
        private bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            if (certificate == null) return false;
            using (X509Certificate2 cert2 = new X509Certificate2(certificate))
            {
                // Chỉ cho phép kết nối nếu chứng chỉ có Issuer và Subject hợp lệ theo hệ thống PKI nội bộ
                return cert2.Issuer.Contains("UIT_ECC_RootCA") && cert2.Subject.Contains("RemoteMonitorServer");
            }
        }

        private string GetLocalIPAddress()
        {
            string localIP = "127.0.0.1";
            try
            {
                foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (item.OperationalStatus == OperationalStatus.Up && item.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                    {
                        var props = item.GetIPProperties();
                        if (props.GatewayAddresses.Count > 0)
                        {
                            foreach (UnicastIPAddressInformation ip in props.UnicastAddresses)
                            {
                                if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                                    return ip.Address.ToString();
                            }
                        }
                    }
                }
            }
            catch { }
            return localIP;
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            _computer?.Close(); // Giải phóng Handle driver cảm biến để tránh rò rỉ bộ nhớ (Memory Leak)
        }

        #endregion
    }

    /// <summary>
    /// Triển khai Design Pattern: Visitor (theo chuẩn của thư viện LibreHardwareMonitor).
    /// Hỗ trợ duyệt đệ quy và Update() đúng cách trên các cây phần cứng phức tạp (như Intel Alder Lake hybrid architecture).
    /// </summary>
    public class UpdateVisitor : IVisitor
    {
        public void VisitComputer(IComputer computer) => computer.Traverse(this);

        public void VisitHardware(IHardware hardware)
        {
            hardware.Update();
            foreach (IHardware sub in hardware.SubHardware)
                sub.Accept(this);
        }

        public void VisitSensor(ISensor sensor) { }
        public void VisitParameter(IParameter parameter) { }
    }
}