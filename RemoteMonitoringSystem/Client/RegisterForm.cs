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
using Newtonsoft.Json;

namespace Client
{
    /// <summary>
    /// Giao diện đăng ký tài khoản hệ thống.
    /// Triển khai cơ chế sinh chuỗi ngẫu nhiên an toàn mật mã (CSPRNG) để tạo Salt.
    /// </summary>
    public partial class RegisterForm : Form
    {
        public RegisterForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Sinh chuỗi Salt ngẫu nhiên sử dụng RNGCryptoServiceProvider.
        /// </summary>
        private string GenerateSalt(int length = 16)
        {
            byte[] saltBytes = new byte[length];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }

        private async void btnSubmitRegister_Click(object sender, EventArgs e)
        {
            string username = txtRegUsername.Text.Trim();
            string password = txtRegPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin định danh!", "Lỗi xác thực", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Xác nhận mật khẩu không trùng khớp!", "Lỗi tính toàn vẹn", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string salt = GenerateSalt();
            string passwordHash = ComputeSha256Hash(password + salt);

            try
            {
                using (TcpClient client = new TcpClient("127.0.0.1", 8888))
                using (NetworkStream netStream = client.GetStream())
                using (SslStream sslStream = new SslStream(netStream, false, ValidateServerCertificate))
                {
                    X509Certificate2 clientCertificate = new X509Certificate2("ClientCertECC.pfx", "NT106.Q23");
                    X509CertificateCollection clientCerts = new X509CertificateCollection(new X509Certificate[] { clientCertificate });

                    await sslStream.AuthenticateAsClientAsync("RemoteMonitorServer", clientCerts, SslProtocols.Tls12, false);

                    using (StreamReader reader = new StreamReader(sslStream, Encoding.UTF8))
                    using (StreamWriter writer = new StreamWriter(sslStream, Encoding.UTF8) { AutoFlush = true })
                    {
                        var regRequest = new
                        {
                            Type = "REGISTER",
                            Username = username,
                            Password = passwordHash,
                            Salt = salt
                        };

                        await writer.WriteLineAsync(JsonConvert.SerializeObject(regRequest));
                        string response = await reader.ReadLineAsync();

                        if (response == "REGISTER_OK")
                        {
                            MessageBox.Show("Cấp phát định danh tài khoản thành công!", "Thông báo hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Định danh đã tồn tại hoặc hệ thống từ chối yêu cầu!", "Lỗi khởi tạo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Xảy ra lỗi trong tiến trình giao tiếp mạng: {ex.Message}", "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

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

        private bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            if (certificate == null) return false;
            X509Certificate2 cert2 = new X509Certificate2(certificate);
            return cert2.Issuer.Contains("UIT_ECC_RootCA") && cert2.Subject.Contains("RemoteMonitorServer");
        }
    }
}