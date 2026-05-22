using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Linq;

namespace Server
{
    public partial class ServerForm : Form
    {
        private TcpListener listener;
        private DatabaseHelper db;

        // Quản lý phiên giám sát (Thread-safe collection)
        private static ConcurrentDictionary<string, AgentSession> connectedAgents = new ConcurrentDictionary<string, AgentSession>();

        // Quản lý phân quyền dựa trên luồng kết nối TCP
        private static ConcurrentDictionary<StreamWriter, string> connectedRoles = new ConcurrentDictionary<StreamWriter, string>();

        public ServerForm()
        {
            InitializeComponent();
            db = new DatabaseHelper();
        }

        private async void ServerForm_Load_1(object sender, EventArgs e)
        {
            LogToScreen("=== HỆ THỐNG ĐIỀU PHỐI TRUNG TÂM (SECURE MODE) ===");
            await Task.Run(() => StartServerAsync());
        }

        /// <summary>
        /// Khởi tạo TcpListener và liên tục lắng nghe các kết nối từ máy trạm.
        /// </summary>
        private async Task StartServerAsync()
        {
            try
            {
                listener = new TcpListener(IPAddress.Any, 8888);
                listener.Start();
                LogToScreen("Máy chủ đang lắng nghe tại cổng 8888...");

                while (true)
                {
                    TcpClient client = await listener.AcceptTcpClientAsync().ConfigureAwait(false);
                    _ = HandleClientAsync(client);
                }
            }
            catch (Exception ex)
            {
                LogToScreen($"[LỖI HỆ THỐNG] {ex.Message}");
            }
        }

        /// <summary>
        /// Xử lý độc lập từng kết nối Client thông qua đường hầm bảo mật mTLS.
        /// </summary>
        private async Task HandleClientAsync(TcpClient client)
        {
            StreamWriter writer = null;
            string agentShareCode = null;

            try
            {
                using (SslStream sslStream = new SslStream(client.GetStream(), false, ValidateClientCertificate))
                {
                    // Trình chứng chỉ máy chủ và yêu cầu xác thực lẫn nhau (Mutual Authentication)
                    X509Certificate2 cert = new X509Certificate2("ServerCertECC.pfx", "NT106.Q23");
                    await sslStream.AuthenticateAsServerAsync(cert, true, SslProtocols.Tls12, false);

                    using (StreamReader reader = new StreamReader(sslStream, Encoding.UTF8))
                    {
                        writer = new StreamWriter(sslStream, Encoding.UTF8) { AutoFlush = true };

                        string requestJson;
                        while ((requestJson = await reader.ReadLineAsync()) != null)
                        {
                            dynamic data = JsonConvert.DeserializeObject(requestJson);
                            string type = data.Type;

                            switch (type)
                            {
                                #region --- LUỒNG XÁC THỰC VÀ ĐĂNG KÝ TÀI KHOẢN ---
                                case "GET_SALT":
                                    string salt = db.GetUserSalt((string)data.Username);
                                    await writer.WriteLineAsync(salt != null ? $"SALT {salt}" : "NOT_FOUND");
                                    break;

                                case "REGISTER":
                                    bool isCreated = db.CreateUser((string)data.Username, (string)data.Password, (string)data.Salt);
                                    await writer.WriteLineAsync(isCreated ? "REGISTER_OK" : "REGISTER_FAIL");
                                    LogToScreen($"[HỆ THỐNG] Khởi tạo tài khoản mới: {(string)data.Username}");
                                    break;

                                case "LOGIN":
                                    string role = db.ValidateUser((string)data.Username, (string)data.Password);
                                    if (role != null)
                                    {
                                        await writer.WriteLineAsync($"LOGIN_OK {role}");
                                        connectedRoles[writer] = role;
                                        LogToScreen($"[HỆ THỐNG] Cấp phép truy cập: {(string)data.Username} (Quyền hạn: {role})");
                                    }
                                    else
                                    {
                                        await writer.WriteLineAsync("LOGIN_FAIL");
                                    }
                                    break;

                                case "IDENTIFY_DASHBOARD":
                                    string clientRole = (string)data.Role;
                                    connectedRoles[writer] = clientRole;
                                    LogToScreen($"[HỆ THỐNG] Nhận diện kết nối Dashboard: {(string)data.Username} (Quyền hạn: {clientRole})");
                                    break;
                                #endregion

                                #region --- QUẢN LÝ PHIÊN GIÁM SÁT (AGENT SESSIONS) ---
                                case "REGISTER_AGENT":
                                    {
                                        string agentId = GenerateStaticID((string)data.MachineName);
                                        string sessionPass = GenerateRandomPassword();

                                        AgentSession session = new AgentSession
                                        {
                                            ShareCode = agentId,
                                            SessionPassword = sessionPass,
                                            MachineName = (string)data.MachineName,
                                            IP = (string)data.IP,
                                            Username = (string)data.Username,
                                            Writer = writer,
                                            LatestData = "",
                                            IsOnline = true
                                        };

                                        connectedAgents[agentId] = session;
                                        agentShareCode = agentId;

                                        _ = Task.Run(() => db.SaveClient(agentId, session.MachineName, session.IP));

                                        await writer.WriteLineAsync($"REGISTER_OK|{agentId}|{sessionPass}");
                                        LogToScreen($"[AGENT ONLINE] {session.MachineName} | ID: {agentId} | Pass: {sessionPass}");
                                        break;
                                    }

                                case "AUTH_AGENT":
                                    {
                                        string targetId = (string)data.TargetID;
                                        string targetPass = (string)data.Password;

                                        if (connectedAgents.TryGetValue(targetId, out var targetSession))
                                        {
                                            if (!targetSession.IsOnline)
                                                await writer.WriteLineAsync("AUTH_FAIL|Máy trạm đang trong trạng thái OFFLINE!");
                                            else if (targetSession.SessionPassword == targetPass)
                                                await writer.WriteLineAsync("AUTH_OK");
                                            else
                                                await writer.WriteLineAsync("AUTH_FAIL|Mật khẩu xác thực phiên không chính xác!");
                                        }
                                        else
                                        {
                                            await writer.WriteLineAsync("AUTH_FAIL|Không tìm thấy mã định danh máy trạm!");
                                        }
                                        break;
                                    }
                                #endregion

                                #region --- ĐIỀU PHỐI DỮ LIỆU TÀI NGUYÊN HỆ THỐNG ---
                                case "PUSH_RESOURCE":
                                    {
                                        string shareCode = (string)data.ShareCode;
                                        if (connectedAgents.TryGetValue(shareCode, out var session))
                                        {
                                            session.LatestData = requestJson; 

                                            _ = Task.Run(() => {
                                                db.SaveResourceHistory(
                                                    shareCode,
                                                    Convert.ToDouble(data.Cpu),
                                                    Convert.ToDouble(data.Ram),
                                                    Convert.ToDouble(data.Disk),
                                                    Convert.ToDouble(data.NetDown),
                                                    Convert.ToDouble(data.NetUp),
                                                    (string)data.AppList
                                                );
                                            });
                                        }
                                        break;
                                    }

                                case "GET_LATEST_BY_CODE":
                                    {
                                        string shareCode = (string)data.ShareCode;
                                        string reqPass = (string)data.Password;

                                        if (connectedAgents.TryGetValue(shareCode, out var session))
                                        {
                                            if (!session.IsOnline)
                                            {
                                                await writer.WriteLineAsync("AGENT_OFFLINE");
                                            }
                                            else if (IsAdmin(writer) || session.SessionPassword == reqPass)
                                            {
                                                await writer.WriteLineAsync($"LATEST_DATA {session.LatestData}");
                                            }
                                            else
                                            {
                                                await writer.WriteLineAsync("SESSION_EXPIRED");
                                            }
                                        }
                                        else
                                        {
                                            await writer.WriteLineAsync("NO_DATA");
                                        }
                                        break;
                                    }

                                case "GET_ALL_CLIENTS":
                                    {
                                        if (!IsAdmin(writer))
                                        {
                                            await writer.WriteLineAsync("ACCESS_DENIED");
                                            break;
                                        }

                                        string result = string.Join(";", connectedAgents.Values
                                            .Select(s => $"{s.ShareCode}|{s.MachineName}|{s.Username}|{(s.IsOnline ? "ONLINE" : "OFFLINE")}"));

                                        await writer.WriteLineAsync(result);
                                        break;
                                    }
                                #endregion

                                #region --- XỬ LÝ NHẬT KÝ SỰ KIỆN & LỆNH ĐIỀU KHIỂN ---
                                case "PUSH_REALTIME_LOG":
                                    {
                                        string shareCode = (string)data.ShareCode;
                                        if (connectedAgents.TryGetValue(shareCode, out var session))
                                        {
                                            session.PendingLogs.Enqueue(requestJson);
                                            if (session.PendingLogs.Count > 50)
                                                session.PendingLogs.TryDequeue(out _);

                                            _ = Task.Run(() => {
                                                db.SaveEventLog(
                                                    shareCode,
                                                    (string)data.LogType,
                                                    (string)data.Source,
                                                    (string)data.Message,
                                                    (string)data.Time
                                                );
                                            });
                                        }
                                        break;
                                    }

                                case "GET_EVENT_LOGS":
                                    {
                                        string targetCode = (string)data.TargetShareCode;
                                        string reqPass = (string)data.Password;

                                        if (connectedAgents.TryGetValue(targetCode, out var session))
                                        {
                                            if (!session.IsOnline)
                                            {
                                                await writer.WriteLineAsync("NO_NEW_LOGS");
                                            }
                                            else if (IsAdmin(writer) || session.SessionPassword == reqPass)
                                            {
                                                var logsToDeliver = new System.Collections.Generic.List<string>();
                                                while (session.PendingLogs.TryDequeue(out string singleLog))
                                                {
                                                    logsToDeliver.Add(singleLog);
                                                }

                                                if (logsToDeliver.Count > 0)
                                                    await writer.WriteLineAsync(JsonConvert.SerializeObject(new { Type = "EVENT_LOGS_DATA", Logs = logsToDeliver }));
                                                else
                                                    await writer.WriteLineAsync("NO_NEW_LOGS");
                                            }
                                            else
                                            {
                                                await writer.WriteLineAsync("SESSION_EXPIRED");
                                            }
                                        }
                                        else
                                        {
                                            await writer.WriteLineAsync("NO_NEW_LOGS");
                                        }
                                        break;
                                    }

                                case "REMOTE_KILL":
                                    {
                                        if (!IsAdmin(writer))
                                        {
                                            await writer.WriteLineAsync("ACCESS_DENIED");
                                            break;
                                        }

                                        string target = (string)data.TargetClientId;
                                        string processName = (string)data.ProcessName;

                                        if (connectedAgents.TryGetValue(target, out var session) && session.IsOnline)
                                        {
                                            await session.Writer.WriteLineAsync(JsonConvert.SerializeObject(new { Type = "KILL_PROCESS", ProcessName = processName }));
                                            await writer.WriteLineAsync("KILL_SENT");
                                        }
                                        else
                                        {
                                            await writer.WriteLineAsync("CLIENT_NOT_FOUND");
                                        }
                                        break;
                                    }
                                    #endregion
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogToScreen($"[NGẮT KẾT NỐI] Mất luồng giao tiếp TCP - {ex.Message}");
            }
            finally
            {
                // Cập nhật trạng thái Offline khi Client đóng kết nối thay vì xóa khỏi hệ thống
                if (agentShareCode != null)
                {
                    if (connectedAgents.TryGetValue(agentShareCode, out var session))
                    {
                        if (session.Writer == writer)
                        {
                            session.IsOnline = false;
                            LogToScreen($"[AGENT OFFLINE] {session.MachineName} đã ngắt kết nối chia sẻ.");
                        }
                    }
                }

                if (writer != null)
                {
                    connectedRoles.TryRemove(writer, out _);
                    writer.Close();
                }

                client.Close();
            }
        }

        #region --- TIỆN ÍCH BẢO MẬT & XỬ LÝ HỆ THỐNG ---

        /// <summary>
        /// Khởi tạo mã định danh tĩnh (Static ID) thông qua thuật toán băm SHA-256 dựa trên định danh phần cứng.
        /// </summary>
        private string GenerateStaticID(string machineName)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(machineName + "SecretUITKey"));
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < 3; i++) sb.Append(hashBytes[i].ToString("X2"));
                return sb.ToString();
            }
        }

        /// <summary>
        /// Tạo mật khẩu cấp phát động (Dynamic Session Password) ngẫu nhiên gồm 6 chữ số.
        /// </summary>
        private string GenerateRandomPassword()
        {
            Random rnd = new Random();
            return rnd.Next(100000, 999999).ToString();
        }

        private bool IsAdmin(StreamWriter writer)
        {
            return connectedRoles.TryGetValue(writer, out string role) && role == "Admin";
        }

        /// <summary>
        /// Giao thức xác thực chứng chỉ số mTLS từ Client gửi lên.
        /// Kiểm tra tính hợp lệ của Root CA và phân loại Subject.
        /// </summary>
        private bool ValidateClientCertificate(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors errors)
        {
            if (cert == null)
            {
                LogToScreen("[BẢO MẬT] Từ chối: Client không cung cấp chứng chỉ định danh (Vui lòng kiểm tra Root CA).");
                return false;
            }

            X509Certificate2 cert2 = new X509Certificate2(cert);
            bool isValidIssuer = cert2.Issuer.Contains("UIT_ECC_RootCA");
            bool isValidSubject = cert2.Subject.Contains("RemoteMonitorClient") || cert2.Subject.Contains("RemoteMonitorServer");

            if (!isValidIssuer || !isValidSubject)
            {
                LogToScreen($"[BẢO MẬT] Từ chối kết nối: Thông tin chứng chỉ (Issuer/Subject) không hợp lệ.");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Ghi nhận nhật ký hệ thống ra màn hình giao diện (An toàn đa luồng).
        /// </summary>
        private void LogToScreen(string msg)
        {
            if (rtbLogs.InvokeRequired)
            {
                rtbLogs.Invoke(new Action(() => LogToScreen(msg)));
                return;
            }
            rtbLogs.AppendText($"{DateTime.Now:HH:mm:ss} - {msg}{Environment.NewLine}");
            rtbLogs.ScrollToCaret();
        }
        #endregion
    }
}