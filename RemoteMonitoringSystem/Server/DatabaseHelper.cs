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

        /// <summary>
        /// Xác thực thông tin đăng nhập của người dùng.
        /// </summary>
        /// <returns>Chuỗi phân quyền (Role) nếu thành công, null nếu thất bại.</returns>
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
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("[LỖI DB - ValidateUser] " + ex.Message);
                    return null;
                }
            }
        }

        /// <summary>
        /// Truy xuất chuỗi Salt ngẫu nhiên được cấp phát riêng cho từng tài khoản.
        /// </summary>
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
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("[LỖI DB - GetUserSalt] " + ex.Message);
                    return null;
                }
            }
        }

        /// <summary>
        /// Khởi tạo tài khoản người dùng mới vào hệ thống.
        /// </summary>
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
                catch
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Truy xuất danh sách toàn bộ các máy trạm (Clients) đã từng kết nối vào hệ thống.
        /// </summary>
        /// <returns>Chuỗi JSON chứa danh sách máy trạm.</returns>
        public string GetAllClientsList()
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT ClientId, MachineName, IP, LastActive FROM Clients";
                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        var clientsList = new System.Collections.Generic.List<object>();
                        while (reader.Read())
                        {
                            clientsList.Add(new
                            {
                                ClientId = reader["ClientId"].ToString(),
                                MachineName = reader["MachineName"].ToString(),
                                IP = reader["IP"].ToString()
                            });
                        }
                        return JsonConvert.SerializeObject(clientsList);
                    }
                }
                catch
                {
                    return "[]";
                }
            }
        }
    }
}