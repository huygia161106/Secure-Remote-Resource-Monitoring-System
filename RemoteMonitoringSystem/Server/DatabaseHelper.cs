using System;
using System.Data.SQLite;
using System.IO;
using Newtonsoft.Json;

namespace Server
{
    /// <summary>
    /// Lớp tiện ích quản lý các tương tác với cơ sở dữ liệu SQLite.
    /// Ứng dụng Parameterized Queries để ngăn chặn lỗi SQL Injection.
    /// </summary>
    public class DatabaseHelper
    {
        private readonly string connectionString = "Data Source=RemoteMonitor.db;Version=3;";

        public DatabaseHelper()
        {
            if (!File.Exists("RemoteMonitor.db"))
            {
                System.Diagnostics.Debug.WriteLine("[LỖI HỆ THỐNG] Không tìm thấy tệp cơ sở dữ liệu RemoteMonitor.db!");
            }
        }

        public string ValidateUser(string username, string passwordHash)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT Role FROM Users WHERE Username = @user AND PasswordHash = @pass";
                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@user", username);
                        cmd.Parameters.AddWithValue("@pass", passwordHash);

                        object result = cmd.ExecuteScalar();
                        return result != null ? result.ToString() : null;
                    }
                }
                catch (Exception ex) { System.Diagnostics.Debug.WriteLine("[LỖI DB - ValidateUser] " + ex.Message); return null; }
            }
        }

        public string GetUserSalt(string username)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT Salt FROM Users WHERE Username = @user";
                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@user", username);
                        object result = cmd.ExecuteScalar();
                        return result != null ? result.ToString() : null;
                    }
                }
                catch (Exception ex) { System.Diagnostics.Debug.WriteLine("[LỖI DB - GetUserSalt] " + ex.Message); return null; }
            }
        }

        public bool CreateUser(string username, string passwordHash, string salt)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO Users (Username, PasswordHash, Salt) VALUES (@user, @pass, @salt)";
                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@user", username);
                        cmd.Parameters.AddWithValue("@pass", passwordHash);
                        cmd.Parameters.AddWithValue("@salt", salt);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
                catch { return false; }
            }
        }


        /// <summary>
        /// Lưu hoặc cập nhật thông tin thiết bị vào bảng Clients (Sử dụng lệnh Upsert).
        /// </summary>
        public void SaveClient(string shareCode, string machineName, string ip)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = @"
                        INSERT INTO Clients (ShareCode, MachineName, IP, LastActive) 
                        VALUES (@code, @name, @ip, CURRENT_TIMESTAMP)
                        ON CONFLICT(ShareCode) DO UPDATE SET 
                        IP = excluded.IP, LastActive = CURRENT_TIMESTAMP;";
                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@code", shareCode);
                        cmd.Parameters.AddWithValue("@name", machineName);
                        cmd.Parameters.AddWithValue("@ip", ip);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex) { System.Diagnostics.Debug.WriteLine("[LỖI DB - SaveClient] " + ex.Message); }
            }
        }

        /// <summary>
        /// Ghi nhận lịch sử tài nguyên phần cứng định kỳ.
        /// </summary>
        public void SaveResourceHistory(string shareCode, double cpu, double ram, double disk, double netDown, double netUp, string appList)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = @"INSERT INTO ResourceHistory (ShareCode, CpuPercent, RamPercent, DiskPercent, NetworkDown, NetworkUp, AppList) 
                                     VALUES (@code, @cpu, @ram, @disk, @netDown, @netUp, @appList)";
                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@code", shareCode);
                        cmd.Parameters.AddWithValue("@cpu", cpu);
                        cmd.Parameters.AddWithValue("@ram", ram);
                        cmd.Parameters.AddWithValue("@disk", disk);
                        cmd.Parameters.AddWithValue("@netDown", netDown);
                        cmd.Parameters.AddWithValue("@netUp", netUp);
                        cmd.Parameters.AddWithValue("@appList", appList);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex) { System.Diagnostics.Debug.WriteLine("[LỖI DB - SaveResource] " + ex.Message); }
            }
        }

        /// <summary>
        /// Ghi nhận nhật ký sự kiện an ninh hệ thống.
        /// </summary>
        public void SaveEventLog(string shareCode, string logType, string source, string message, string logTime)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = @"INSERT INTO EventLogs (ShareCode, LogType, Source, Message, LogTime) 
                                     VALUES (@code, @type, @source, @msg, @time)";
                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@code", shareCode);
                        cmd.Parameters.AddWithValue("@type", logType);
                        cmd.Parameters.AddWithValue("@source", source);
                        cmd.Parameters.AddWithValue("@msg", message);
                        cmd.Parameters.AddWithValue("@time", logTime);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex) { System.Diagnostics.Debug.WriteLine("[LỖI DB - SaveEventLog] " + ex.Message); }
            }
        }
    }
}