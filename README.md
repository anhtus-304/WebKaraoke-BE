# WebKaraoke-BE
WebKaraoke API - Backend Documentation
📋 Giới thiệu
WebKaraoke API là hệ thống backend cho ứng dụng quản lý Karaoke, cung cấp các API để quản lý phòng hát, đặt phòng, hóa đơn, khách hàng và các tính năng liên quan.

🚀 Công nghệ sử dụng
.NET 8.0 - Framework

Entity Framework Core - ORM

SQL Server - Database

JWT - Authentication

Swagger - API Documentation

📁 Cấu trúc Project
text
WebKaraoke/
├── WebKaraoke.API/          # Main API project
├── WebKaraoke.Business/     # Business logic layer
├── WebKaraoke.Data/         # Data access layer
├── WebKaraoke.DTO/          # Data transfer objects
└── WebKaraoke.Entities/     # Database entities
⚙️ Yêu cầu hệ thống
.NET 8.0 SDK

SQL Server (LocalDB hoặc SQL Server Express)

Visual Studio 2022 hoặc VS Code

🔧 Cài đặt và chạy project
1. Clone và mở project
bash
git clone <repository-url>
cd Backend_WebKara
2. Cấu hình database
Cập nhật connection string trong appsettings.json:

json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=WebKaraokeDB;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
3. Tạo và cập nhật database
bash
# Chạy trong thư mục WebKaraoke.API
dotnet ef database update
Hoặc sử dụng Package Manager Console trong Visual Studio:

bash
Update-Database
4. Chạy ứng dụng
bash
# Build project
dotnet build

# Chạy ứng dụng
dotnet run --project WebKaraoke.API
Hoặc sử dụng Visual Studio:

Mở file WebKaraoke.sln

Set WebKaraoke.API làm Startup Project

Nhấn F5 hoặc Ctrl+F5 để chạy

🌐 Truy cập ứng dụng
API: https://localhost:7000

Swagger UI: https://localhost:7000/swagger

HTTP: http://localhost:5000

🔐 Authentication & Authorization
Hệ thống sử dụng JWT Bearer Token với 3 role chính:

Admin: Toàn quyền hệ thống

NhanVien: Quản lý đặt phòng, hóa đơn

Khach: Khách hàng sử dụng dịch vụ

Tạo tài khoản mặc định (nếu cần)
Chạy script SQL để tạo tài khoản mặc định:

sql
INSERT INTO TaiKhoan (Username, Password, Role) VALUES
('admin', 'hashed_password', 'Admin'),
('nhanvien', 'hashed_password', 'NhanVien');
📊 API Endpoints chính
Authentication
POST /api/TaiKhoan/dang-nhap - Đăng nhập

POST /api/TaiKhoan/dang-ky - Đăng ký tài khoản

Quản lý phòng
GET /api/Phong - Lấy danh sách phòng

GET /api/Phong/trong - Lấy phòng còn trống

POST /api/Phong - Tạo phòng mới (Admin)

Đặt phòng
GET /api/DatPhong/khach-hang - Lấy đặt phòng của khách (Khach)

POST /api/DatPhong - Tạo đặt phòng

PUT /api/DatPhong/{id}/huy - Hủy đặt phòng

PUT /api/DatPhong/{id}/xac-nhan - Xác nhận đặt phòng (NV)

Hóa đơn
GET /api/HoaDon - Lấy danh sách hóa đơn (NV, Admin)

POST /api/HoaDon - Tạo hóa đơn (NV, Admin)

PUT /api/HoaDon/{id}/thanh-toan - Thanh toán hóa đơn

Khách hàng
GET /api/KhachHang/profile - Lấy thông tin cá nhân

PUT /api/KhachHang/profile - Cập nhật thông tin

🗃️ Database Schema
Các bảng chính:

Phong - Thông tin phòng hát

KhachHang - Thông tin khách hàng

DatPhong - Thông tin đặt phòng

HoaDon - Hóa đơn thanh toán

ChiTietHoaDon - Chi tiết hóa đơn

MonAnNuocUong - Menu đồ ăn, thức uống

TaiKhoan - Tài khoản đăng nhập

🐛 Xử lý lỗi thường gặp
Lỗi build
bash
# Clean và rebuild
dotnet clean
dotnet build

# Hoặc xóa thư mục bin/obj
rmdir /S /Q bin obj
dotnet build
Lỗi database
bash
# Tạo migration mới
dotnet ef migrations add InitialCreate
dotnet ef database update

# Reset database
dotnet ef database drop
dotnet ef database update
Lỗi port đang sử dụng
bash
# Tìm process sử dụng port
netstat -ano | findstr :5000

# Kill process
taskkill /PID <PID> /F
📝 Development Notes
Thêm migration mới
bash
dotnet ef migrations add <MigrationName>
dotnet ef database update
Chạy với môi trường Development
bash
dotnet run --environment Development
Chạy với môi trường Production
bash
dotnet run --environment Production
🔄 Workflow phát triển
Cập nhật Entities/DTOs nếu cần

Tạo Migration: dotnet ef migrations add <name>

Cập nhật Database: dotnet ef database update

Test API qua Swagger UI

Commit và push code

📞 Hỗ trợ
Nếu gặp vấn đề trong quá trình cài đặt và chạy project, vui lòng:

Kiểm tra lại connection string

Đảm bảo .NET 8.0 SDK đã được cài đặt

Kiểm tra SQL Server đang chạy

Clean và rebuild project

🎯 Next Steps
Cấu hình CORS cho frontend

Triển khai lên server

Cấu hình HTTPS

Setup logging và monitoring
