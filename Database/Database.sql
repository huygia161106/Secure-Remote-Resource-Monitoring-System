-- ==========================================================================
-- SCRIPT KHỞI TẠO CƠ SỞ DỮ LIỆU TOÀN VẸN (PHIÊN BẢN GIÁM SÁT AN NINH CHUYÊN SÂU)
-- Hệ thống: Quản trị Tài nguyên & Nhật ký Hoạt động Máy trạm từ xa
-- Nền tảng đích: SQLite 3
-- ==========================================================================

-- Kích hoạt cơ chế kiểm tra ràng buộc khóa ngoại (Foreign Key Constraints)
PRAGMA foreign_keys = ON;

-- ==========================================================================
-- BẢNG 1: Users (Quản lý Danh tính và Phân quyền Cơ chế RBAC)
-- ==========================================================================
CREATE TABLE IF NOT EXISTS Users (
    UserId          INTEGER PRIMARY KEY AUTOINCREMENT,
    Username        TEXT UNIQUE NOT NULL,
    PasswordHash    TEXT NOT NULL,
    Salt            TEXT NOT NULL,
    Role            TEXT DEFAULT 'User',
    CreatedDate     DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- ==========================================================================
-- BẢNG 2: Clients (Quản lý Danh sách Máy trạm Mục tiêu)
-- Sử dụng cấu trúc định danh Static ID (6 ký tự Hex) thay thế cho ID tự tăng
-- ==========================================================================
CREATE TABLE IF NOT EXISTS Clients (
    ShareCode       TEXT PRIMARY KEY,
    MachineName     TEXT NOT NULL,
    IP              TEXT,
    OwnerUserId     INTEGER,
    LastActive      DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (OwnerUserId) REFERENCES Users(UserId) ON DELETE CASCADE
);

-- ==========================================================================
-- BẢNG 3: ResourceHistory (Lưu trữ Telemetry Hiệu năng)
-- ==========================================================================
CREATE TABLE IF NOT EXISTS ResourceHistory (
    Id              INTEGER PRIMARY KEY AUTOINCREMENT,
    ShareCode       TEXT NOT NULL,
    Timestamp       DATETIME DEFAULT CURRENT_TIMESTAMP,
    CpuPercent      REAL,
    RamPercent      REAL,
    DiskPercent     REAL,
    NetworkDown     REAL,
    NetworkUp       REAL,
    AppList         TEXT,
    FOREIGN KEY (ShareCode) REFERENCES Clients(ShareCode) ON DELETE CASCADE
);

-- ==========================================================================
-- BẢNG 4: EventLogs (Nhật ký Sự kiện An ninh và Kiểm toán Hệ thống - Audit)
-- Lưu vết hành vi tương tác ứng dụng nhạy cảm (CMD, PowerShell...)
-- ==========================================================================
CREATE TABLE IF NOT EXISTS EventLogs (
    Id              INTEGER PRIMARY KEY AUTOINCREMENT,
    ShareCode       TEXT NOT NULL,
    LogType         TEXT NOT NULL, -- Phân loại cấp độ: Info, Warning, Error
    Source          TEXT,          -- Tiến trình phát sinh (VD: powershell.exe)
    Message         TEXT NOT NULL, -- Nội dung chi tiết hoạt động hoặc cảnh báo
    LogTime         TEXT,          -- Thời gian ghi nhận tại Endpoint (HH:mm:ss)
    CreatedDate     DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (ShareCode) REFERENCES Clients(ShareCode) ON DELETE CASCADE
);

-- ==========================================================================
-- KHỞI TẠO DỮ LIỆU GỐC (SEED DATA)
-- Khởi tạo tài khoản Quản trị viên mặc định phục vụ cơ chế kiểm thử
-- Mật khẩu thô gốc: admin -> Cơ chế: SHA256(password + salt)
-- ==========================================================================
INSERT INTO Users (Username, PasswordHash, Salt, Role)
VALUES (
    'admin',
    'b1676334c7a3905649ecc3ad90ba2ab18c4b3f5f430c5754a7ba84744b6c6954',
    'UuctoxqY1HamIqACTRcipQ==',
    'Admin'
);