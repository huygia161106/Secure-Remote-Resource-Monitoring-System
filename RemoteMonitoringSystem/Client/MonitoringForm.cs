using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Newtonsoft.Json;

namespace Client
{
    /// <summary>
    /// Khối chức năng (Module) Tác nhân chạy ngầm trên máy trạm.
    /// Nhiệm vụ: Thu thập dữ liệu đo lường (Telemetry) phần cứng, giám sát hành vi ứng dụng và thực thi lệnh từ xa.
    /// </summary>
    public partial class MonitoringForm : Form
    {
        #region --- KHAI BÁO TÀI NGUYÊN & LUỒNG MẠNG ---

        // Giám sát tài nguyên phần cứng
        private PerformanceCounter cpuCounter;
        private long prevBytesReceived = 0;
        private long prevBytesSent = 0;

        // Giám sát hành vi thao tác cửa sổ
        private string lastActiveWindow = "";
        private CancellationTokenSource activityCts;

        // Định danh & Thông tin mạng của thiết bị
        private string currentShareCode = "";
        private string machineName = Environment.MachineName;
        private string ipAddress = "127.0.0.1";
        private string currentUsername;

        // Quản lý kênh giao tiếp mạng bảo mật (mTLS)
        private TcpClient currentClient;
        private SslStream currentSslStream;
        private StreamWriter currentWriter;
        private StreamReader currentReader;

        // Cờ kiểm soát và Khóa đồng bộ hóa nhằm đảm bảo an toàn đa luồng (Thread-safe)
        private bool _isSending = false;
        private SemaphoreSlim networkLock = new SemaphoreSlim(1, 1);

        // Danh sách trắng (Whitelist) các tiến trình lõi của hệ điều hành được bảo vệ khỏi lệnh tắt từ xa
        private readonly string[] protectedProcesses = {
            "svchost", "explorer", "csrss", "wininit", "smss", "services", "lsass", "system"
        };

        #endregion

        public MonitoringForm(string username)
        {
            InitializeComponent();
            currentUsername = username;
            ipAddress = GetLocalIPAddress();
            InitializeCounters();
        }

        private void InitializeCounters()
        {
            try
            {
                cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
                cpuCounter.NextValue();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khởi tạo bộ đếm hiệu năng hệ thống: {ex.Message}", "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnStart_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = false;
            lblStatus.Text = "Trạng thái: Đang khởi tạo đường hầm mã hóa mTLS...";
            lblStatus.ForeColor = System.Drawing.Color.DarkOrange;

            bool isConnected = await ConnectToServerAsync();

            if (isConnected)
            {
                btnStop.Enabled = true;
                lblStatus.Text = $"Trạng thái: Hệ thống đang chia sẻ dữ liệu | ID: {currentShareCode}";
                lblStatus.ForeColor = System.Drawing.Color.Green;
                timerPush.Start();
            }
            else
            {
                btnStart.Enabled = true;
            }
        }

        private async Task<bool> ConnectToServerAsync()
        {
            try
            {
                currentClient = new TcpClient { NoDelay = true };
                var connectTask = currentClient.ConnectAsync("127.0.0.1", 8888);

                if (await Task.WhenAny(connectTask, Task.Delay(2000)).ConfigureAwait(false) != connectTask)
                    throw new TimeoutException("Máy chủ trung tâm không phản hồi tín hiệu.");

                await connectTask.ConfigureAwait(false);

                currentSslStream = new SslStream(currentClient.GetStream(), false, ValidateServerCertificate);
                X509Certificate2 clientCertificate = new X509Certificate2("ClientCertECC.pfx", "NT106.Q23");
                X509CertificateCollection clientCerts = new X509CertificateCollection(new X509Certificate[] { clientCertificate });

                await currentSslStream.AuthenticateAsClientAsync("RemoteMonitorServer", clientCerts, SslProtocols.Tls12, false).ConfigureAwait(false);

                currentWriter = new StreamWriter(currentSslStream, Encoding.UTF8) { AutoFlush = true };
                currentReader = new StreamReader(currentSslStream, Encoding.UTF8);

                var regData = new { Type = "REGISTER_AGENT", MachineName = machineName, IP = ipAddress, Username = currentUsername };
                await currentWriter.WriteLineAsync(JsonConvert.SerializeObject(regData));

                string response = await currentReader.ReadLineAsync();
                if (response != null && response.StartsWith("REGISTER_OK|"))
                {
                    string[] parts = response.Split('|');
                    currentShareCode = parts[1];
                    string sessionPass = parts[2];

                    this.Invoke((Action)(() => {
                        lblShareCode.Text = $"ID: {currentShareCode}   |   PASS: {sessionPass}";
                    }));
                }
                else
                {
                    throw new Exception("Hệ thống từ chối cấp phát mã định danh phiên.");
                }

                // Kích hoạt tiến trình giám sát hành vi và lắng nghe lệnh từ máy chủ
                StartActivityTracker();
                _ = Task.Run(() => ListenForCommands());

                return true;
            }
            catch (Exception ex)
            {
                this.Invoke((Action)(() => {
                    lblStatus.Text = $"Lỗi giao tiếp mạng: {ex.Message}";
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                }));
                return false;
            }
        }

        #region --- KHỐI THU THẬP DỮ LIỆU ĐO LƯỜNG & CẢNH BÁO SỚM ---

        /// <summary>
        /// Luồng xử lý định kỳ thu thập thông số CPU, RAM, Ổ cứng, Mạng và Danh sách ứng dụng.
        /// Áp dụng cơ chế khóa SemaphoreSlim để ngăn chặn xung đột khi truyền tải qua mạng.
        /// </summary>
        private async void timerPush_Tick(object sender, EventArgs e)
        {
            if (_isSending) return;
            _isSending = true;

            try
            {
                if (currentSslStream == null || currentClient == null || !currentClient.Connected)
                {
                    btnStop_Click(null, null);
                    return;
                }

                float cpuVal = await Task.Run(() => cpuCounter.NextValue()).ConfigureAwait(false);
                string cpu = Math.Round(cpuVal, 1).ToString();

                Microsoft.VisualBasic.Devices.ComputerInfo ci = new Microsoft.VisualBasic.Devices.ComputerInfo();
                double totalRamMB = ci.TotalPhysicalMemory / (1024.0 * 1024.0);
                double availableRamMB = ci.AvailablePhysicalMemory / (1024.0 * 1024.0);
                double usedRamMB = totalRamMB - availableRamMB;
                string ramUsage = Math.Round((usedRamMB / totalRamMB) * 100, 1).ToString();

                DriveInfo drive = new DriveInfo("C");
                double diskFreePercent = (double)drive.AvailableFreeSpace / drive.TotalSize * 100;
                string disk = Math.Round(100 - diskFreePercent, 1).ToString();

                long currentReceived = 0, currentSent = 0;
                foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (ni.OperationalStatus == OperationalStatus.Up)
                    {
                        currentReceived += ni.GetIPv4Statistics().BytesReceived;
                        currentSent += ni.GetIPv4Statistics().BytesSent;
                    }
                }
                double downloadSpeedKBps = (prevBytesReceived != 0) ? (currentReceived - prevBytesReceived) / 2.0 / 1024.0 : 0;
                double uploadSpeedKBps = (prevBytesSent != 0) ? (currentSent - prevBytesSent) / 2.0 / 1024.0 : 0;
                prevBytesReceived = currentReceived;
                prevBytesSent = currentSent;

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
                    catch { } // Bỏ qua ngoại lệ khi truy cập thông tin các tiến trình hệ thống cấp cao
                }
                string appList = appBuilder.ToString().TrimEnd(';');
                if (string.IsNullOrEmpty(appList)) appList = "NONE";

                var resourceData = new
                {
                    Type = "PUSH_RESOURCE",
                    ShareCode = currentShareCode,
                    Cpu = cpu,
                    Ram = ramUsage,
                    Disk = disk,
                    NetDown = Math.Round(downloadSpeedKBps, 1).ToString(),
                    NetUp = Math.Round(uploadSpeedKBps, 1).ToString(),
                    MachineName = machineName,
                    IP = ipAddress,
                    AppList = appList
                };

                await networkLock.WaitAsync();
                try
                {
                    if (currentWriter != null && currentClient.Connected)
                    {
                        await currentWriter.WriteLineAsync(JsonConvert.SerializeObject(resourceData));

                        // Module Cảnh báo sớm: Tự động phát hiện và gửi tín hiệu báo động khi phần cứng quá tải
                        if (cpuVal >= 80)
                        {
                            var cpuAlert = new { Type = "PUSH_REALTIME_LOG", ShareCode = currentShareCode, LogType = "Warning", Source = "System Monitor", Time = DateTime.Now.ToString("HH:mm:ss"), Message = $"[BẢO MẬT HỆ THỐNG] Ngưỡng CPU vượt mức an toàn ({cpu}%)!" };
                            await currentWriter.WriteLineAsync(JsonConvert.SerializeObject(cpuAlert));
                        }

                        double ramPercent = (usedRamMB / totalRamMB) * 100;
                        if (ramPercent >= 80)
                        {
                            var ramAlert = new { Type = "PUSH_REALTIME_LOG", ShareCode = currentShareCode, LogType = "Warning", Source = "System Monitor", Time = DateTime.Now.ToString("HH:mm:ss"), Message = $"[BẢO MẬT HỆ THỐNG] Ngưỡng RAM vượt mức an toàn ({Math.Round(ramPercent, 1)}%)!" };
                            await currentWriter.WriteLineAsync(JsonConvert.SerializeObject(ramAlert));
                        }
                    }
                }
                finally { networkLock.Release(); }
            }
            catch { this.Invoke((Action)(() => { btnStop_Click(null, null); lblStatus.Text = "Lỗi đồng bộ: Máy chủ trung tâm ngắt kết nối."; lblStatus.ForeColor = System.Drawing.Color.Red; })); }
            finally { _isSending = false; }
        }
        #endregion

        #region --- KHỐI TƯƠNG TÁC HỆ THỐNG (WIN32 API) & PHÁT HIỆN MỐI ĐE DỌA ---

        // Gọi thư viện lõi của Windows để truy xuất thông tin cửa sổ ứng dụng
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        [DllImport("user32.dll")]
        static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, out uint ProcessId);

        /// <summary>
        /// Khởi chạy tiến trình nền nhằm theo dõi hành vi tương tác cửa sổ của người dùng cuối.
        /// </summary>
        private void StartActivityTracker()
        {
            activityCts = new CancellationTokenSource();
            Task.Run(async () => {
                while (!activityCts.Token.IsCancellationRequested)
                {
                    CheckActiveWindow();
                    await Task.Delay(1000);
                }
            });
        }

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

                uint pid;
                GetWindowThreadProcessId(handle, out pid);
                Process p = Process.GetProcessById((int)pid);
                string processName = p.ProcessName.ToLower();

                string currentActivity = $"{processName}|{windowTitle}";

                // Kỹ thuật Hướng sự kiện (Event-Driven): Chỉ tiêu tốn băng thông gửi log khi phát hiện sự kiện chuyển đổi cửa sổ
                if (currentActivity != lastActiveWindow)
                {
                    lastActiveWindow = currentActivity;
                    string logType = "Info";

                    // Bộ lọc nhận diện hành vi sử dụng công cụ hệ thống có rủi ro (Kỹ thuật tấn công LotL - Living off the Land)
                    if (processName == "cmd" || processName == "powershell" || processName == "windowsterminal" ||
                        processName == "regedit" || processName == "taskmgr" || processName == "mmc")
                    {
                        logType = "Error";
                    }

                    var logPacket = new
                    {
                        Type = "PUSH_REALTIME_LOG",
                        ShareCode = currentShareCode,
                        LogType = logType,
                        Source = processName + ".exe",
                        Time = DateTime.Now.ToString("HH:mm:ss"),
                        Message = logType == "Error"
                            ? $"[CẢNH BÁO AN NINH] Kích hoạt công cụ quản trị hệ thống: {windowTitle}!"
                            : $"[GIÁM SÁT] Phiên thao tác chuyển sang: {windowTitle}"
                    };

                    await networkLock.WaitAsync();
                    try
                    {
                        if (currentWriter != null && currentClient.Connected)
                        {
                            await currentWriter.WriteLineAsync(JsonConvert.SerializeObject(logPacket));
                        }
                    }
                    catch { }
                    finally { networkLock.Release(); }
                }
            }
            catch { }
        }
        #endregion

        #region --- KHỐI PHẢN ỨNG SỰ CỐ (ĐIỀU KHIỂN TẮT TIẾN TRÌNH TỪ XA) ---

        /// <summary>
        /// Liên tục lắng nghe và thực thi các chỉ thị điều khiển từ Máy chủ trung tâm (Mô hình Command and Control).
        /// </summary>
        private async Task ListenForCommands()
        {
            try
            {
                string cmdJson;
                while ((cmdJson = await currentReader.ReadLineAsync()) != null)
                {
                    dynamic cmd = JsonConvert.DeserializeObject(cmdJson);
                    if (cmd.Type == "KILL_PROCESS")
                    {
                        string pName = ((string)cmd.ProcessName).Replace(".exe", "").ToLower();

                        // Đối chiếu với Danh sách trắng (Whitelist) để ngăn chặn hành vi tự hủy hệ điều hành
                        bool isProtected = false;
                        foreach (string proc in protectedProcesses)
                        {
                            if (pName == proc) { isProtected = true; break; }
                        }
                        if (isProtected) continue;

                        Process[] processes = Process.GetProcessesByName(pName);
                        foreach (var process in processes)
                        {
                            try { process.Kill(); } catch { }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.Invoke((Action)(() => {
                    btnStop_Click(null, null);
                    lblStatus.Text = $"Luồng thực thi lệnh điều khiển bị gián đoạn: {ex.Message}";
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                }));
            }
        }
        #endregion

        private void btnStop_Click(object sender, EventArgs e)
        {
            timerPush.Stop();
            activityCts?.Cancel();
            currentWriter?.Close(); currentReader?.Close();
            currentSslStream?.Close(); currentClient?.Close();

            btnStart.Enabled = true; btnStop.Enabled = false;
            lblStatus.Text = "Trạng thái: Đã ngắt kết nối chia sẻ dữ liệu hệ thống.";
            lblStatus.ForeColor = System.Drawing.Color.Red;
        }

        private bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            if (certificate == null) return false;
            X509Certificate2 cert2 = new X509Certificate2(certificate);
            return cert2.Issuer.Contains("UIT_ECC_RootCA") && cert2.Subject.Contains("RemoteMonitorServer");
        }

        /// <summary>
        /// Trích xuất địa chỉ IPv4 từ giao diện mạng đang hoạt động (Active Network Interface) để định danh máy trạm.
        /// </summary>
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
                                if (ip.Address.AddressFamily == AddressFamily.InterNetwork) return ip.Address.ToString();
                            }
                        }
                    }
                }
            }
            catch { }
            return localIP;
        }
    }
}