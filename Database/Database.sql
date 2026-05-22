-- ==========================================
-- SCRIPT KHỞI TẠO CƠ SỞ DỮ LIỆU SQLITE (V2 - SECURE ARCHITECTURE)
-- Đồ án: Hệ thống Quản trị Tài nguyên & Giám sát An toàn (Remote Monitor)
-- ==========================================

-- Bật tính năng kiểm tra ràng buộc Khóa ngoại (Foreign Key)
PRAGMA foreign_keys = ON;

-- ==========================================
-- BẢNG 1: LƯU TRỮ THÔNG TIN TÀI KHOẢN (AUTHENTICATION)
-- ==========================================
CREATE TABLE IF NOT EXISTS Users (
    UserId          INTEGER PRIMARY KEY AUTOINCREMENT, 
    Username        TEXT UNIQUE NOT NULL,            
    PasswordHash    TEXT NOT NULL,               
    Salt            TEXT NOT NULL,
    Role            TEXT DEFAULT 'User',                 
    CreatedDate     DATETIME DEFAULT CURRENT_TIMESTAMP 
);

-- ==========================================
-- BẢNG 2: QUẢN LÝ DANH SÁCH MÁY TRẠM (TEAMVIEWER CORE)
-- Thay thế ClientId (Int) bằng ShareCode (Static ID - VD: 7BAC02)
-- ==========================================
CREATE TABLE IF NOT EXISTS Clients (
    ShareCode       TEXT PRIMARY KEY,  -- Khóa chính là chuỗi định danh 6 ký tự
    MachineName     TEXT NOT NULL,        
    IP              TEXT,                                    
    OwnerUserId     INTEGER,                        
    LastActive      DATETIME DEFAULT CURRENT_TIMESTAMP,                        
    FOREIGN KEY (OwnerUserId) REFERENCES Users(UserId) ON DELETE CASCADE
);

-- ==========================================
-- BẢNG 3: LƯU TRỮ LỊCH SỬ TÀI NGUYÊN (TELEMETRY HISTORY)
-- Dùng để vẽ biểu đồ và truy xuất dữ liệu phần cứng theo thời gian
-- ==========================================
CREATE TABLE IF NOT EXISTS ResourceHistory (
    Id              INTEGER PRIMARY KEY AUTOINCREMENT,       
    ShareCode       TEXT NOT NULL,  -- Liên kết với máy trạm qua ShareCode                 
    Timestamp       DATETIME DEFAULT CURRENT_TIMESTAMP, 
    CpuPercent      REAL,                                
    RamPercent      REAL,                                
    DiskPercent     REAL,                               
    NetworkDown     REAL,                               
    NetworkUp       REAL,                                 
    AppList         TEXT,       
    FOREIGN KEY (ShareCode) REFERENCES Clients(ShareCode) ON DELETE CASCADE
);

-- ==========================================
-- BẢNG 4: LƯU TRỮ NHẬT KÝ AN NINH (EVENT LOGS / AUDIT)
-- Dùng để lưu vết các hành vi rủi ro (Mở CMD, Taskmgr) và quá tải tài nguyên
-- ==========================================
CREATE TABLE IF NOT EXISTS EventLogs (
    Id              INTEGER PRIMARY KEY AUTOINCREMENT,
    ShareCode       TEXT NOT NULL,
    LogType         TEXT NOT NULL,  -- Phân loại: Info, Warning, Error
    Source          TEXT,           -- Nguồn phát sinh (VD: windowsterminal.exe, System Monitor)
    Message         TEXT NOT NULL,  -- Nội dung cảnh báo/hoạt động
    LogTime         TEXT,           -- Thời gian gửi từ Client (HH:mm:ss)
    CreatedDate     DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (ShareCode) REFERENCES Clients(ShareCode) ON DELETE CASCADE
);

-- ==========================================
-- DỮ LIỆU KHỞI TẠO (MẶC ĐỊNH CHO QUẢN TRỊ VIÊN)
-- ==========================================
INSERT INTO Users (Username, PasswordHash, Salt, Role) 
VALUES ('admin', 'b1676334c7a3905649ecc3ad90ba2ab18c4b3f5f430c5754a7ba84744b6c6954', 'UuctoxqY1HamIqACTRcipQ==', 'Admin');