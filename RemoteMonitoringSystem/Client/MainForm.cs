using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    /// <summary>
    /// Giao diện giám sát trung tâm (Dashboard).
    /// Áp dụng mô hình điều khiển truy cập dựa trên vai trò (Role-Based Access Control) và Asynchronous Polling.
    /// </summary>
    public partial class MainForm : Form
    {
        #region --- KHAI BÁO BIẾN HỆ THỐNG & LUỒNG MẠNG ---

        private TcpClient client;
        private SslStream sslStream;
        private StreamReader reader;
        private StreamWriter writer;

        private string currentShareCode = "";
        private string currentSessionPassword = "";
        private string currentUserRole;
        private string currentUsername;

        // Quản lý khóa đồng bộ hóa luồng mạng
        private System.Threading.SemaphoreSlim networkLock = new System.Threading.SemaphoreSlim(1, 1);

        private int sortColumn = -1;
        private SortOrder sortOrder = SortOrder.None;
        #endregion

        #region --- KHỞI TẠO VÀ CẤU HÌNH GIAO DIỆN (UI CONFIGURATION) ---

        public MainForm(string username, string role)
        {
            InitializeComponent();
            SetupChartAppearance();

            currentUsername = username;
            currentUserRole = role;

            this.listView1.ColumnClick += new ColumnClickEventHandler(listView1_ColumnClick);
            this.Load += MainForm_Load;
            this.Resize += MainForm_Resize;

            SetupAutoScaleUI();

            lbUsername.Text = $"{currentUsername} ({currentUserRole})";
            ApplySecurityPolicies();
        }

        /// <summary>
        /// Cấu hình thuộc tính Anchor để tối ưu hóa hiển thị trên các độ phân giải màn hình khác nhau (Responsive UI).
        /// </summary>
        private void SetupAutoScaleUI()
        {
            if (guna2TabControl1 != null) guna2TabControl1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            if (listView1 != null) listView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            if (lvEventLogs != null) lvEventLogs.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            if (chart1 != null) chart1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (lvEventLogs != null && lvEventLogs.Columns.Count >= 4)
            {
                int takeWidth = lvEventLogs.Width - (lvEventLogs.Columns[0].Width + lvEventLogs.Columns[1].Width + lvEventLogs.Columns[2].Width);
                if (takeWidth > 200) lvEventLogs.Columns[3].Width = takeWidth - 25;
            }
        }

        /// <summary>
        /// Áp dụng chính sách bảo mật giao diện dựa trên định danh phân quyền (RBAC).
        /// </summary>
        private void ApplySecurityPolicies()
        {
            if (currentUserRole != "Admin")
            {
                if (guna2TabControl1.TabPages.Count > 1) guna2TabControl1.TabPages.RemoveAt(1);

                txtShareCode.Visible = true;
                txtTargetPassword.Visible = true;
                btnConnectCode.Visible = true;

                if (menuEndTaskToolStripMenuItem != null) menuEndTaskToolStripMenuItem.Enabled = false;
                this.Text = "Hệ thống Quản trị Tài nguyên - [USER MODE]";
            }
            else
            {
                txtShareCode.Visible = false;
                txtTargetPassword.Visible = false;
                btnConnectCode.Visible = false;

                this.Text = "Hệ thống Quản trị Tài nguyên - [ADMIN MODE]";
            }
        }
        #endregion

        #region --- XỬ LÝ KẾT NỐI VÀ ĐỒNG BỘ DỮ LIỆU BẤT ĐỒNG BỘ ---

        private async void MainForm_Load(object sender, EventArgs e)
        {
            bool connected = await ConnectToServerAsync();
            if (connected) timerFetchData.Start();
        }

        private async Task<bool> ConnectToServerAsync()
        {
            try
            {
                client = new TcpClient("127.0.0.1", 8888);
                NetworkStream netStream = client.GetStream();

                sslStream = new SslStream(netStream, false, ValidateServerCertificate);

                X509Certificate2 clientCertificate = new X509Certificate2("ClientCertECC.pfx", "NT106.Q23");
                X509CertificateCollection clientCerts = new X509CertificateCollection(new X509Certificate[] { clientCertificate });

                await sslStream.AuthenticateAsClientAsync("RemoteMonitorServer", clientCerts, SslProtocols.Tls12, false);

                reader = new StreamReader(sslStream, Encoding.UTF8);
                writer = new StreamWriter(sslStream, Encoding.UTF8) { AutoFlush = true };

                var identifyRequest = new { Type = "IDENTIFY_DASHBOARD", Username = currentUsername, Role = currentUserRole };
                await writer.WriteLineAsync(JsonConvert.SerializeObject(identifyRequest));

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Xảy ra lỗi trong quá trình đàm phán mTLS: {ex.Message}", "Lỗi bảo mật", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        /// <summary>
        /// Luồng thực thi Polling lấy dữ liệu Telemetry và Event Log định kỳ từ Máy chủ.
        /// Quản lý lỗi ngắt kết nối và phiên làm việc hết hạn.
        /// </summary>
        private async void timerFetchData_Tick(object sender, EventArgs e)
        {
            try
            {
                if (writer == null || reader == null) return;
                if (string.IsNullOrEmpty(currentShareCode)) return;

                await networkLock.WaitAsync();
                try
                {
                    var fetchRequest = new { Type = "GET_LATEST_BY_CODE", ShareCode = currentShareCode, Password = currentSessionPassword };
                    await writer.WriteLineAsync(JsonConvert.SerializeObject(fetchRequest));

                    string response = await reader.ReadLineAsync();

                    if (response == "AGENT_OFFLINE")
                    {
                        this.Invoke(new Action(() => { lblip.Text = "Trạng thái: Máy trạm mục tiêu đang OFFLINE."; }));
                    }
                    else if (response == "SESSION_EXPIRED")
                    {
                        timerFetchData.Stop();
                        currentShareCode = "";
                        MessageBox.Show("Phiên làm việc đã hết hạn do máy trạm khởi tạo lại liên kết. Vui lòng cập nhật mật khẩu phiên!", "Cảnh báo an ninh", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    else if (response == "ACCESS_DENIED")
                    {
                        timerFetchData.Stop();
                        MessageBox.Show("Truy cập bị từ chối do vi phạm chính sách phân quyền!", "Lỗi bảo mật", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else if (response != null && response.StartsWith("LATEST_DATA"))
                    {
                        string payload = response.Substring("LATEST_DATA ".Length).Trim();
                        if (!payload.StartsWith("{")) return;

                        dynamic data = JsonConvert.DeserializeObject(payload);
                        if (data != null)
                        {
                            UpdateResourceChart(Convert.ToDouble(data.Cpu), Convert.ToDouble(data.Ram), Convert.ToDouble(data.Disk));
                            UpdateSystemInfo((string)data.MachineName, (string)data.IP, (string)data.NetDown, (string)data.NetUp);
                            string appList = (string)data.AppList;
                            UpdateAppList(string.IsNullOrEmpty(appList) || appList == "NONE" ? "NONE" : appList);
                        }
                    }
                    else if (response == "NO_DATA")
                    {
                        this.Invoke(new Action(() => { lblip.Text = "Trạng thái: Đang khởi tạo bộ đệm dữ liệu..."; }));
                    }

                    // Tải dữ liệu nhật ký sự kiện thời gian thực (Event Logs)
                    var fetchLogsRequest = new { Type = "GET_EVENT_LOGS", TargetShareCode = currentShareCode, Password = currentSessionPassword };
                    await writer.WriteLineAsync(JsonConvert.SerializeObject(fetchLogsRequest));

                    string logResponse = await reader.ReadLineAsync();

                    if (logResponse != null && logResponse.StartsWith("{"))
                    {
                        dynamic logData = JsonConvert.DeserializeObject(logResponse);
                        if (logData.Type == "EVENT_LOGS_DATA")
                        {
                            this.Invoke(new Action(() =>
                            {
                                lvEventLogs.BeginUpdate();
                                foreach (string rawLog in logData.Logs)
                                {
                                    dynamic logObj = JsonConvert.DeserializeObject(rawLog);
                                    ListViewItem item = new ListViewItem((string)logObj.LogType);

                                    if ((string)logObj.LogType == "Error") item.ForeColor = System.Drawing.Color.Red;
                                    else if ((string)logObj.LogType == "Warning") item.ForeColor = System.Drawing.Color.DarkOrange;

                                    item.SubItems.Add((string)logObj.Source);
                                    item.SubItems.Add((string)logObj.Time);
                                    item.SubItems.Add((string)logObj.Message);

                                    lvEventLogs.Items.Insert(0, item);
                                }
                                while (lvEventLogs.Items.Count > 100) lvEventLogs.Items.RemoveAt(100);
                                lvEventLogs.EndUpdate();
                            }));
                        }
                    }
                }
                finally { networkLock.Release(); }
            }
            catch (Exception ex)
            {
                timerFetchData.Stop();
                MessageBox.Show($"Mất kết nối tới máy chủ điều phối: {ex.Message}", "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region --- MODULE CHUYỂN ĐỔI NGỮ CẢNH VÀ QUẢN TRỊ TRẠNG THÁI ---

        /// <summary>
        /// Xóa bỏ bộ đệm hiển thị (UI Buffer) khi chuyển đổi mục tiêu giám sát.
        /// </summary>
        private void ClearDashboardData()
        {
            chart1.Series["CPU"].Points.Clear();
            chart1.Series["RAM"].Points.Clear();
            listView1.Items.Clear();
            if (lvEventLogs != null) lvEventLogs.Items.Clear();

            progressBar1.Value = 0;
            progressBar3.Value = 0;
            if (progressBar4 != null) progressBar4.Value = 0;

            lblPercentCPU.Text = "0%";
            lblPercentRam.Text = "0%";
            if (lblPercentDisk != null) lblPercentDisk.Text = "0%";

            lblMachineName.Text = "-";
            lblip.Text = "Đang thiết lập kết nối dữ liệu...";
            lblNetDown.Text = "0 KB/s";
            lblNetUp.Text = "0 KB/s";
        }

        private async void btnConnectCode_Click(object sender, EventArgs e)
        {
            string code = txtShareCode.Text.Trim();
            string pass = txtTargetPassword.Text.Trim();

            if (string.IsNullOrEmpty(code))
            {
                MessageBox.Show("Vui lòng nhập định danh liên kết (Share Code)!", "Lỗi cú pháp", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (currentUserRole != "Admin")
                {
                    if (string.IsNullOrEmpty(pass))
                    {
                        MessageBox.Show("Vui lòng cung cấp mật khẩu phiên bản (Session Password)!", "Lỗi xác thực", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    var authReq = new { Type = "AUTH_AGENT", TargetID = code, Password = pass };
                    await writer.WriteLineAsync(JsonConvert.SerializeObject(authReq));

                    string authRes = await reader.ReadLineAsync();
                    if (authRes != "AUTH_OK")
                    {
                        string reason = authRes.Contains("|") ? authRes.Split('|')[1] : "Thông tin xác thực bị từ chối!";
                        MessageBox.Show($"Lỗi kết nối: {reason}", "Lỗi bảo mật", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                ClearDashboardData();

                currentShareCode = code;
                currentSessionPassword = pass;

                MessageBox.Show($"Xác thực thành công. Đang giám sát thiết bị: {code}", "Thông báo hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (!timerFetchData.Enabled) timerFetchData.Start();
            }
            catch (Exception ex) { MessageBox.Show($"Lỗi xử lý luồng: {ex.Message}", "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void dgvClients_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string selectedId = dgvClients.Rows[e.RowIndex].Cells[0].Value?.ToString();
                if (!string.IsNullOrEmpty(selectedId))
                {
                    ClearDashboardData();

                    currentShareCode = selectedId;
                    currentSessionPassword = ""; // Đặc quyền Admin không yêu cầu Session Password

                    guna2TabControl1.SelectedIndex = 0;
                    if (!timerFetchData.Enabled) timerFetchData.Start();
                }
            }
        }
        #endregion

        #region --- THAO TÁC XỬ LÝ GIAO DIỆN (UI MANIPULATION) ---

        public void UpdateResourceChart(double cpuPercent, double ramPercent, double diskPercent)
        {
            if (this.chart1.InvokeRequired) { this.chart1.Invoke(new Action(() => UpdateResourceChart(cpuPercent, ramPercent, diskPercent))); return; }

            string currentTime = DateTime.Now.ToString("HH:mm:ss");
            chart1.Series["CPU"].Points.AddXY(currentTime, cpuPercent);
            chart1.Series["RAM"].Points.AddXY(currentTime, ramPercent);

            if (chart1.Series["CPU"].Points.Count > 30)
            {
                chart1.Series["CPU"].Points.RemoveAt(0);
                chart1.Series["RAM"].Points.RemoveAt(0);
            }

            progressBar1.Value = (int)Math.Min(100, Math.Max(0, cpuPercent));
            progressBar3.Value = (int)Math.Min(100, Math.Max(0, ramPercent));
            if (progressBar4 != null) progressBar4.Value = (int)Math.Min(100, Math.Max(0, diskPercent));

            lblPercentCPU.Text = $"{Math.Round(cpuPercent, 1)}%";
            lblPercentRam.Text = $"{Math.Round(ramPercent, 1)}%";
            if (lblPercentDisk != null) lblPercentDisk.Text = $"{Math.Round(diskPercent, 1)}%";
        }

        public void UpdateAppList(string appListData)
        {
            if (this.listView1.InvokeRequired) { this.listView1.Invoke(new Action(() => UpdateAppList(appListData))); return; }

            if (string.IsNullOrWhiteSpace(appListData) || appListData == "NONE")
            {
                listView1.Items.Clear();
                return;
            }

            string selectedProcess = listView1.SelectedItems.Count > 0 ? listView1.SelectedItems[0].Text : "";
            int topItemIndex = listView1.TopItem != null ? listView1.TopItem.Index : 0;

            listView1.BeginUpdate();
            listView1.Items.Clear();

            string[] apps = appListData.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string app in apps)
            {
                string[] details = app.Split('|');
                if (details.Length >= 3)
                {
                    ListViewItem item = new ListViewItem(details[0]);
                    item.SubItems.Add(details[1]);
                    item.SubItems.Add(details[2]);
                    listView1.Items.Add(item);
                }
            }

            if (sortColumn != -1) listView1.Sort();
            if (listView1.Items.Count > topItemIndex) listView1.TopItem = listView1.Items[topItemIndex];

            if (!string.IsNullOrEmpty(selectedProcess))
            {
                foreach (ListViewItem item in listView1.Items)
                {
                    if (item.Text == selectedProcess) { item.Selected = true; break; }
                }
            }
            listView1.EndUpdate();
        }

        public void UpdateSystemInfo(string machineName, string ip, string netDown, string netUp)
        {
            if (this.InvokeRequired) { this.Invoke(new Action(() => UpdateSystemInfo(machineName, ip, netDown, netUp))); return; }
            lblMachineName.Text = machineName; lblip.Text = ip;
            lblNetDown.Text = $"{netDown} KB/s"; lblNetUp.Text = $"{netUp} KB/s";
        }

        private async void btnRefreshList_Click(object sender, EventArgs e)
        {
            if (writer == null || reader == null) return;

            await networkLock.WaitAsync();
            try
            {
                var request = new { Type = "GET_ALL_CLIENTS" };
                await writer.WriteLineAsync(JsonConvert.SerializeObject(request));
                string response = await reader.ReadLineAsync();

                if (string.IsNullOrEmpty(response)) return;

                if (response == "ACCESS_DENIED")
                {
                    MessageBox.Show("Truy cập bị từ chối do vi phạm phân quyền hệ thống!", "Lỗi truy cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                dgvClients.Rows.Clear();
                string[] clients = response.Split(';');
                foreach (string client in clients)
                {
                    if (string.IsNullOrWhiteSpace(client)) continue;
                    string[] parts = client.Split('|');
                    if (parts.Length >= 4) dgvClients.Rows.Add(parts[0], parts[1], parts[2], parts[3]);
                }
            }
            catch (Exception ex) { MessageBox.Show($"Lỗi cập nhật danh sách thiết bị: {ex.Message}", "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            finally { networkLock.Release(); }
        }

        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column == sortColumn) sortOrder = (sortOrder == SortOrder.Ascending) ? SortOrder.Descending : SortOrder.Ascending;
            else { sortColumn = e.Column; sortOrder = SortOrder.Ascending; }

            listView1.ListViewItemSorter = new ListViewItemComparer(sortColumn, sortOrder);
            listView1.Sort();
        }
        #endregion

        #region --- LỆNH NGOẠI VI (INCIDENT RESPONSE) & TIỆN ÍCH ---

        private async void EndTaskToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                string pName = listView1.SelectedItems[0].Text;
                string windowTitle = listView1.SelectedItems[0].SubItems[1].Text;

                if (string.IsNullOrEmpty(currentShareCode))
                {
                    MessageBox.Show("Vui lòng chỉ định một thiết bị mục tiêu từ danh sách quản trị!", "Lỗi định tuyến", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var confirm = MessageBox.Show($"Xác nhận thực thi chỉ thị gián đoạn tiến trình (Remote Kill):\n- Ứng dụng: {windowTitle} ({pName})\n- Máy trạm: [{currentShareCode}]", "Hệ thống phản ứng sự cố", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (confirm == DialogResult.Yes)
                {
                    try
                    {
                        var request = new { Type = "REMOTE_KILL", TargetClientId = currentShareCode, ProcessName = pName };
                        if (writer != null)
                        {
                            await writer.WriteLineAsync(JsonConvert.SerializeObject(request));
                            MessageBox.Show($"Chỉ thị hệ thống đã được gửi đi thành công tới thiết bị đích.", "Cập nhật lệnh", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (Exception ex) { MessageBox.Show($"Lỗi truyền tải chỉ thị điều khiển: {ex.Message}", "Lỗi mạng", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
            }
        }

        public class ListViewItemComparer : System.Collections.IComparer
        {
            private int col;
            private SortOrder order;
            public ListViewItemComparer(int column, SortOrder order) { col = column; this.order = order; }

            public int Compare(object x, object y)
            {
                int returnVal = -1;
                string strX = ((ListViewItem)x).SubItems[col].Text;
                string strY = ((ListViewItem)y).SubItems[col].Text;

                if (col == 2) // Cột bộ nhớ (Memory allocation)
                {
                    double numX = ExtractNumber(strX); double numY = ExtractNumber(strY);
                    returnVal = numX.CompareTo(numY);
                }
                else returnVal = String.Compare(strX, strY);

                if (order == SortOrder.Descending) returnVal *= -1;
                return returnVal;
            }

            private double ExtractNumber(string input)
            {
                string numberOnly = System.Text.RegularExpressions.Regex.Replace(input, "[^0-9.]", "");
                if (double.TryParse(numberOnly, out double result)) return result;
                return 0;
            }
        }

        private bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            if (certificate == null) return false;
            X509Certificate2 cert2 = new X509Certificate2(certificate);
            return cert2.Issuer.Contains("UIT_ECC_RootCA") && cert2.Subject.Contains("RemoteMonitorServer");
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            writer?.Close(); client?.Close();
            Application.Exit();
        }
        private void SetupChartAppearance() { }
        #endregion
    }
}