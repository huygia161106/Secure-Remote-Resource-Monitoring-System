using System;
using System.Windows.Forms;

namespace Client
{
    /// <summary>
    /// Giao diện định tuyến tính năng sau quá trình xác thực.
    /// Điều hướng người dùng dựa trên nhu cầu Giám sát (Dashboard) hoặc Cung cấp dữ liệu (Agent).
    /// </summary>
    public partial class ModeSelectionForm : Form
    {
        private string currentRole;
        private string currentUsername;

        public ModeSelectionForm(string username, string role)
        {
            InitializeComponent();
            currentUsername = username;
            currentRole = role;
        }

        private void btnShare_Click(object sender, EventArgs e)
        {
            // Khởi tạo Agent thu thập telemetry
            MonitoringForm form = new MonitoringForm(currentUsername);
            form.Show();
            this.Hide();
        }

        private void btnMonitor_Click(object sender, EventArgs e)
        {
            // Khởi tạo Dashboard quản trị
            MainForm form = new MainForm(currentUsername, currentRole);
            form.Show();
            this.Hide();
        }
    }
}