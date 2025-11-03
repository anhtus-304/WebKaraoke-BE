-- Thêm dữ liệu mẫu cho Phong
INSERT INTO Phong (TenPhong, LoaiPhong, GiaGio, TrangThai) VALUES
('P001', 'VIP', 200000, 'Trong'),
('P002', 'VIP', 200000, 'Trong'),
('P003', 'VIP', 200000, 'Trong'),
('P004', 'Thuong', 150000, 'Trong'),
('P005', 'Thuong', 150000, 'Trong'),
('P006', 'Thuong', 150000, 'Trong');
GO

-- Thêm dữ liệu mẫu cho NhanVien
INSERT INTO NhanVien (HoTen, SoDienThoai, ChucVu) VALUES
(N'Nguyễn Văn Quản Lý', '0901111111', N'Quản lý'),
(N'Trần Thị Nhân Viên', '0902222222', N'Nhân viên'),
(N'Lê Văn Phục Vụ', '0903333333', N'Phục vụ');
GO

-- Thêm dữ liệu mẫu cho KhachHang
INSERT INTO KhachHang (HoTen, SoDienThoai, Email, DiaChi) VALUES
(N'Nguyễn Văn A', '0901234567', 'nguyenvana@email.com', N'123 Đường ABC, Quận 1, TP.HCM'),
(N'Trần Thị B', '0902345678', 'tranthib@email.com', N'456 Đường XYZ, Quận 2, TP.HCM'),
(N'Lê Văn C', '0903456789', 'levanc@email.com', N'789 Đường DEF, Quận 3, TP.HCM');
GO

-- Thêm dữ liệu mẫu cho MonAnNuocUong
-- Đồ ăn
INSERT INTO MonAnNuocUong (TenMon, DonGia, DanhMuc, MoTa) VALUES
(N'Bò khô', 50000, 'DoAn', N'Bò khô cay ngon'),
(N'Hạt điều', 40000, 'DoAn', N'Hạt điều rang muối'),
(N'Khoai tây chiên', 35000, 'DoAn', N'Khoai tây chiên giòn'),
(N'Gà nướng', 80000, 'DoAn', N'Gà nướng muối ớt'),
(N'Bánh mì pate', 25000, 'DoAn', N'Bánh mì pate chả lụa');

-- Nước uống
INSERT INTO MonAnNuocUong (TenMon, DonGia, DanhMuc, MoTa) VALUES
(N'Coca Cola', 25000, 'NuocUong', N'Nước ngọt Coca Cola'),
(N'Pepsi', 25000, 'NuocUong', N'Nước ngọt Pepsi'),
(N'Nước suối', 15000, 'NuocUong', N'Nước suối thiên nhiên'),
(N'Trà đào', 35000, 'NuocUong', N'Trà đào cam sả'),
(N'Cà phê đen', 30000, 'NuocUong', N'Cà phê đen đá'),
(N'Sinh tố bơ', 45000, 'NuocUong', N'Sinh tố bơ tươi');
GO

-- Thêm dữ liệu mẫu cho KhuyenMai
INSERT INTO KhuyenMai (MaKM, TyLeGiam, NgayBatDau, NgayKetThuc, MoTa) VALUES
('WELCOME10', 10.00, '2024-01-01', '2024-12-31', N'Khuyến mãi chào mừng khách hàng mới'),
('VIP20', 20.00, '2024-01-01', '2024-12-31', N'Khuyến mãi cho khách hàng VIP'),
('WEEKEND15', 15.00, '2024-01-01', '2024-12-31', N'Khuyến mãi cuối tuần'),
('HOLIDAY25', 25.00, '2024-12-20', '2024-12-31', N'Khuyến mãi dịp lễ');
GO

-- Thêm tài khoản Admin
INSERT INTO TaiKhoan (Username, MatKhauHash, Role, NhanVienID) VALUES
('admin', 'jGl25bVBBBW96Qi9Te4V37Fnqchz/Eu4qB9vKrRIqRg=', 'Admin', 1);
GO

-- Thêm tài khoản cho khách hàng (mật khẩu là 123456)
INSERT INTO TaiKhoan (Username, MatKhauHash, Role, KhachHangID) VALUES
('nguyenvana@email.com', 'jGl25bVBBBW96Qi9Te4V37Fnqchz/Eu4qB9vKrRIqRg=', 'Khach', 1),
('tranthib@email.com', 'jGl25bVBBBW96Qi9Te4V37Fnqchz/Eu4qB9vKrRIqRg=', 'Khach', 2);
GO
