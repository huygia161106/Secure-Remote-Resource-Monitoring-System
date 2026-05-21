using System.IO;
using System.Collections.Concurrent;

namespace Server
{
    /// <summary>
    /// Lớp đại diện cho một phiên kết nối giám sát từ máy trạm (Agent).
    /// Lưu trữ thông tin định danh, trạng thái và hàng đợi dữ liệu thời gian thực.
    /// </summary>
    public class AgentSession
    {
        public string ShareCode { get; set; }
        public string MachineName { get; set; }
        public string IP { get; set; }

        // Luồng ghi dữ liệu trực tiếp xuống máy trạm thông qua mTLS
        public StreamWriter Writer { get; set; }

        // Dữ liệu tài nguyên phần cứng mới nhất (JSON format)
        public string LatestData { get; set; }
        public string Username { get; set; }

        // Mật khẩu xác thực cấp phát động cho từng phiên (Dynamic Session Password)
        public string SessionPassword { get; set; }

        // Cờ trạng thái kết nối mạng của thiết bị
        public bool IsOnline { get; set; } = true;

        // Hàng đợi an toàn đa luồng (Thread-safe) lưu trữ nhật ký sự kiện
        public ConcurrentQueue<string> PendingLogs { get; set; } = new ConcurrentQueue<string>();
    }
}