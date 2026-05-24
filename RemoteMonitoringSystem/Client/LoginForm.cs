using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    /// <summary>
    /// Giao diện xử lý xác thực người dùng.
    /// Áp dụng giao thức Mutual TLS (mTLS) và cơ chế băm mật khẩu có Salt (SHA-256).
    /// </summary>
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Username và Password!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool isLoginSuccess = false;
            string userRole = "User";

            try
            {
                using (TcpClient client = new TcpClient("192.168.31.198", 8888))
                using (NetworkStream netStream = client.GetStream())
                using (SslStream sslStream = new SslStream(netStream, false, ValidateServerCertificate))
                {
                    // Trình chứng chỉ định danh của Client và thiết lập kênh truyền mã hóa mTLS
                    X509Certificate2 clientCertificate = new X509Certificate2("ClientCertECC.pfx", "NT106.Q23");
                    X509CertificateCollection clientCerts = new X509CertificateCollection(new X509Certificate[] { clientCertificate });

                    await sslStream.AuthenticateAsClientAsync("RemoteMonitorServer", clientCerts, SslProtocols.Tls12, false);

                    using (StreamReader reader = new StreamReader(sslStream, Encoding.UTF8))
                    using (StreamWriter writer = new StreamWriter(sslStream, Encoding.UTF8) { AutoFlush = true })
                    {
                        // Bước 1: Yêu cầu chuỗi Salt của người dùng từ hệ thống
                        var getSaltRequest = new { Type = "GET_SALT", Username = username };
                        await writer.WriteLineAsync(JsonConvert.SerializeObject(getSaltRequest));

                        string saltResponse = await reader.ReadLineAsync();

                        if (string.IsNullOrEmpty(saltResponse) || saltResponse == "NOT_FOUND")
                        {
                            MessageBox.Show("Tài khoản không tồn tại trong hệ thống!", "Lỗi xác thực", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        // Trích xuất Salt gốc (Bỏ tiền tố "SALT ")
                        string actualSalt = saltResponse.Substring(5).Trim();

                        // Bước 2: Băm mật khẩu kết hợp Salt và gửi yêu cầu đăng nhập
                        string passwordHash = ComputeSha256Hash(password + actualSalt);
                        var loginRequest = new { Type = "LOGIN", Username = username, Password = passwordHash };
                        await writer.WriteLineAsync(JsonConvert.SerializeObject(loginRequest));

                        string loginResponse = await reader.ReadLineAsync();

                        if (loginResponse != null && loginResponse.StartsWith("LOGIN_OK"))
                        {
                            isLoginSuccess = true;
                            string[] parts = loginResponse.Split(' ');
                            if (parts.Length > 1)
                            {
                                userRole = parts[1]; // Trích xuất phân quyền (Role-Based Access Control)
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi giao tiếp máy chủ: {ex.Message}", "Lỗi mạng", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (isLoginSuccess)
            {
                ModeSelectionForm modeForm = new ModeSelectionForm(username, userRole);
                modeForm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Mật khẩu truy cập không chính xác!", "Lỗi xác thực", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Thuật toán băm một chiều SHA-256 để bảo vệ tính toàn vẹn của mật khẩu.
        /// </summary>
        private string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        /// <summary>
        /// Giao thức xác thực chứng chỉ máy chủ để phòng tránh tấn công Man-in-the-Middle (MitM).
        /// </summary>
        private bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            if (certificate == null) return false;
            X509Certificate2 cert2 = new X509Certificate2(certificate);
            return cert2.Issuer.Contains("UIT_ECC_RootCA") && cert2.Subject.Contains("RemoteMonitorServer");
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            Application.Exit();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            RegisterForm regForm = new RegisterForm();
            regForm.ShowDialog();
        }
    }
}