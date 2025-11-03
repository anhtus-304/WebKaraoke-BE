-- Stored Procedure 1: Tính tiền phòng chi tiết theo loại phòng
CREATE OR ALTER PROCEDURE sp_TinhTienPhongChiTiet
    @DatPhongID INT,
    @TienPhong DECIMAL(12,2) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @GioBatDau DATETIME, @GioKetThuc DATETIME;
    DECLARE @LoaiPhong NVARCHAR(50), @GiaGio DECIMAL(10,2);
    DECLARE @SoPhut INT, @SoBlock15Phut INT;
    
    -- Lấy thông tin đặt phòng và loại phòng
    SELECT 
        @GioBatDau = dp.GioBatDau, 
        @GioKetThuc = dp.GioKetThuc,
        @LoaiPhong = p.LoaiPhong,
        @GiaGio = p.GiaGio
    FROM DatPhong dp
    INNER JOIN Phong p ON dp.PhongID = p.PhongID
    WHERE dp.DatPhongID = @DatPhongID;
    
    IF @GioBatDau IS NULL OR @GioKetThuc IS NULL
    BEGIN
        SET @TienPhong = 0;
        RETURN;
    END
    
    -- Tính số phút sử dụng
    SET @SoPhut = DATEDIFF(MINUTE, @GioBatDau, @GioKetThuc);
    
    -- Tính số block 15 phút (làm tròn lên)
    SET @SoBlock15Phut = CEILING(@SoPhut / 15.0);
    
    -- Tính tiền phòng theo loại và chính sách giá
    IF @LoaiPhong = 'VIP'
    BEGIN
        -- VIP: 200k/giờ, mỗi block 15 phút = 50,000 VND
        -- Giờ đầu: 200k, các giờ sau: 150k/giờ
        IF @SoBlock15Phut <= 4 -- 1 giờ đầu
            SET @TienPhong = 200000;
        ELSE
            SET @TienPhong = 200000 + ((@SoBlock15Phut - 4) * 37500); -- 37,500 VND/15 phút cho giờ sau
    END
    ELSE IF @LoaiPhong = 'Thuong'
    BEGIN
        -- Thường: 150k/giờ, mỗi block 15 phút = 37,500 VND
        -- Giờ đầu: 150k, các giờ sau: 120k/giờ
        IF @SoBlock15Phut <= 4 -- 1 giờ đầu
            SET @TienPhong = 150000;
        ELSE
            SET @TienPhong = 150000 + ((@SoBlock15Phut - 4) * 30000); -- 30,000 VND/15 phút cho giờ sau
    END
    ELSE
    BEGIN
        -- Mặc định tính theo giờ
        SET @TienPhong = CEILING(@SoPhut / 60.0) * @GiaGio;
    END
    
    -- Đảm bảo không âm
    IF @TienPhong < 0 SET @TienPhong = 0;
END;
GO

-- Stored Procedure 2: Tạo hóa đơn tự động với tính toán tiền phòng
CREATE OR ALTER PROCEDURE sp_TaoHoaDonTuDong
    @DatPhongID INT,
    @NhanVienID INT,
    @MaKhuyenMai NVARCHAR(50) = NULL,
    @HoaDonID INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @TienPhong DECIMAL(12,2);
    DECLARE @KM_ID INT = NULL;
    DECLARE @TyLeGiam DECIMAL(5,2) = 0;
    DECLARE @ErrorCode INT = 0;
    
    BEGIN TRANSACTION;
    
    -- Tính tiền phòng
    EXEC sp_TinhTienPhongChiTiet @DatPhongID, @TienPhong OUTPUT;
    
    -- Lấy thông tin khuyến mãi nếu có
    IF @MaKhuyenMai IS NOT NULL
    BEGIN
        SELECT @KM_ID = KM_ID, @TyLeGiam = TyLeGiam
        FROM KhuyenMai 
        WHERE MaKM = @MaKhuyenMai 
        AND GETDATE() BETWEEN NgayBatDau AND NgayKetThuc;
    END
    
    -- Tạo hóa đơn
    INSERT INTO HoaDon (DatPhongID, NhanVienID, KM_ID, TongTien, TrangThai)
    VALUES (@DatPhongID, @NhanVienID, @KM_ID, @TienPhong, 'ChuaThanhToan');
    
    SET @HoaDonID = SCOPE_IDENTITY();
    
    -- Cập nhật trạng thái đặt phòng
    UPDATE DatPhong 
    SET TrangThai = 'HoanTat'
    WHERE DatPhongID = @DatPhongID;
    
    IF @@ERROR <> 0
    BEGIN
        SET @ErrorCode = @@ERROR;
        ROLLBACK TRANSACTION;
        SET @HoaDonID = -1;
        RETURN @ErrorCode;
    END
    
    COMMIT TRANSACTION;
END;
GO

-- Stored Procedure 3: Thống kê doanh thu theo loại phòng
CREATE OR ALTER PROCEDURE sp_ThongKeDoanhThuTheoLoaiPhong
    @TuNgay DATETIME = NULL,
    @DenNgay DATETIME = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Mặc định tháng hiện tại nếu không có tham số
    IF @TuNgay IS NULL SET @TuNgay = DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE()), 0);
    IF @DenNgay IS NULL SET @DenNgay = DATEADD(DAY, -1, DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE()) + 1, 0));
    
    SELECT 
        p.LoaiPhong,
        COUNT(DISTINCT hd.HoaDonID) AS SoHoaDon,
        SUM(hd.TongTien) AS TongDoanhThu,
        AVG(hd.TongTien) AS DoanhThuTrungBinh,
        COUNT(DISTINCT dp.KhachHangID) AS SoKhachHang,
        SUM(DATEDIFF(MINUTE, dp.GioBatDau, dp.GioKetThuc)) / 60.0 AS TongSoGioSuDung,
        -- Tính tỷ lệ sử dụng
        CAST(COUNT(DISTINCT dp.DatPhongID) AS FLOAT) / 
        NULLIF((SELECT COUNT(*) FROM Phong p2 WHERE p2.LoaiPhong = p.LoaiPhong), 0) * 100 AS TyLeSuDungPhanTram,
        -- Doanh thu/giờ
        CASE 
            WHEN SUM(DATEDIFF(MINUTE, dp.GioBatDau, dp.GioKetThuc)) > 0 
            THEN SUM(hd.TongTien) / (SUM(DATEDIFF(MINUTE, dp.GioBatDau, dp.GioKetThuc)) / 60.0)
            ELSE 0
        END AS DoanhThuTrungBinhTheoGio
    FROM HoaDon hd
    INNER JOIN DatPhong dp ON hd.DatPhongID = dp.DatPhongID
    INNER JOIN Phong p ON dp.PhongID = p.PhongID
    WHERE hd.NgayLap BETWEEN @TuNgay AND @DenNgay
    AND hd.TrangThai = 'DaThanhToan'
    GROUP BY p.LoaiPhong
    ORDER BY TongDoanhThu DESC;
END;
GO

-- Stored Procedure 4: Tìm phòng trống theo loại và thời gian
CREATE OR ALTER PROCEDURE sp_TimPhongTrongTheoLoai
    @LoaiPhong NVARCHAR(50) = NULL,
    @GioBatDau DATETIME,
    @GioKetThuc DATETIME,
    @SoLuongNguoi INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        p.PhongID,
        p.TenPhong,
        p.LoaiPhong,
        p.GiaGio,
        p.TrangThai,
        -- Tính giá ước tính cho khoảng thời gian
        CASE 
            WHEN p.LoaiPhong = 'VIP' THEN
                CASE 
                    WHEN DATEDIFF(MINUTE, @GioBatDau, @GioKetThuc) <= 60 THEN 200000
                    ELSE 200000 + (CEILING((DATEDIFF(MINUTE, @GioBatDau, @GioKetThuc) - 60) / 15.0) * 37500)
                END
            WHEN p.LoaiPhong = 'Thuong' THEN
                CASE 
                    WHEN DATEDIFF(MINUTE, @GioBatDau, @GioKetThuc) <= 60 THEN 150000
                    ELSE 150000 + (CEILING((DATEDIFF(MINUTE, @GioBatDau, @GioKetThuc) - 60) / 15.0) * 30000)
                END
            ELSE p.GiaGio * CEILING(DATEDIFF(MINUTE, @GioBatDau, @GioKetThuc) / 60.0)
        END AS GiaUocTinh
    FROM Phong p
    WHERE p.TrangThai = 'Trong'
    AND (@LoaiPhong IS NULL OR p.LoaiPhong = @LoaiPhong)
    AND NOT EXISTS (
        SELECT 1 
        FROM DatPhong dp 
        WHERE dp.PhongID = p.PhongID 
        AND dp.TrangThai NOT IN ('DaHuy', 'HoanTat')
        AND @GioBatDau < dp.GioKetThuc 
        AND @GioKetThuc > dp.GioBatDau
    )
    ORDER BY 
        CASE WHEN @LoaiPhong IS NOT NULL THEN 0 ELSE 1 END,
        p.LoaiPhong,
        p.GiaGio;
END;
GO

-- Stored Procedure 5: Tính tổng tiền hóa đơn cuối cùng (bao gồm khuyến mãi)
CREATE OR ALTER PROCEDURE sp_TinhTongTienHoaDon
    @HoaDonID INT,
    @TongTienSauGiam DECIMAL(12,2) OUTPUT,
    @TienGiam DECIMAL(12,2) OUTPUT,
    @TyLeGiam DECIMAL(5,2) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @TongTienTruocGiam DECIMAL(12,2);
    DECLARE @KM_ID INT;
    
    -- Lấy tổng tiền và khuyến mãi
    SELECT 
        @TongTienTruocGiam = hd.TongTien,
        @KM_ID = hd.KM_ID,
        @TyLeGiam = ISNULL(km.TyLeGiam, 0)
    FROM HoaDon hd
    LEFT JOIN KhuyenMai km ON hd.KM_ID = km.KM_ID
    WHERE hd.HoaDonID = @HoaDonID;
    
    -- Tính tiền giảm và tổng tiền sau giảm
    SET @TienGiam = @TongTienTruocGiam * (@TyLeGiam / 100);
    SET @TongTienSauGiam = @TongTienTruocGiam - @TienGiam;
    
    -- Đảm bảo không âm
    IF @TongTienSauGiam < 0 SET @TongTienSauGiam = 0;
    IF @TienGiam < 0 SET @TienGiam = 0;
END;
GO

-- Stored Procedure 6: Đặt phòng với kiểm tra tự động
CREATE OR ALTER PROCEDURE sp_DatPhongAnToan
    @KhachHangID INT,
    @PhongID INT,
    @GioBatDau DATETIME,
    @GioKetThuc DATETIME,
    @SoLuongNguoi INT,
    @DatPhongID INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @IsValid BIT = 1;
    DECLARE @ErrorMessage NVARCHAR(500) = '';
    DECLARE @ErrorCode INT = 0;
    
    BEGIN TRANSACTION;
    
    -- Kiểm tra phòng có tồn tại và trống không
    IF NOT EXISTS (SELECT 1 FROM Phong WHERE PhongID = @PhongID AND TrangThai = 'Trong')
    BEGIN
        SET @IsValid = 0;
        SET @ErrorMessage = 'Phòng không tồn tại hoặc không trống.';
        SET @ErrorCode = 50001;
    END
    
    -- Kiểm tra trùng lịch
    IF EXISTS (
        SELECT 1 FROM DatPhong 
        WHERE PhongID = @PhongID 
        AND TrangThai NOT IN ('DaHuy', 'HoanTat')
        AND @GioBatDau < GioKetThuc 
        AND @GioKetThuc > GioBatDau
    )
    BEGIN
        SET @IsValid = 0;
        SET @ErrorMessage = 'Phòng đã được đặt trong khoảng thời gian này.';
        SET @ErrorCode = 50002;
    END
    
    -- Kiểm tra thời gian hợp lệ (ít nhất 1 giờ)
    IF DATEDIFF(MINUTE, @GioBatDau, @GioKetThuc) < 60
    BEGIN
        SET @IsValid = 0;
        SET @ErrorMessage = 'Thời gian đặt phòng tối thiểu là 1 giờ.';
        SET @ErrorCode = 50003;
    END
    
    IF @IsValid = 1
    BEGIN
        -- Thực hiện đặt phòng
        INSERT INTO DatPhong (KhachHangID, PhongID, ThoiGianDat, GioBatDau, GioKetThuc, SoLuongNguoi, TrangThai)
        VALUES (@KhachHangID, @PhongID, GETDATE(), @GioBatDau, @GioKetThuc, @SoLuongNguoi, 'ChoXacNhan');
        
        SET @DatPhongID = SCOPE_IDENTITY();
        
        IF @@ERROR <> 0
        BEGIN
            SET @ErrorCode = @@ERROR;
            ROLLBACK TRANSACTION;
            SET @DatPhongID = -1;
            RETURN @ErrorCode;
        END
        
        COMMIT TRANSACTION;
    END
    ELSE
    BEGIN
        ROLLBACK TRANSACTION;
        RAISERROR(@ErrorMessage, 16, 1);
        SET @DatPhongID = -1;
        RETURN @ErrorCode;
    END
END;
GO

-- Stored Procedure 7: Thanh toán hóa đơn và cập nhật điểm
CREATE OR ALTER PROCEDURE sp_ThanhToanHoaDon
    @HoaDonID INT,
    @SoDiemSuDung INT = 0,
    @ThanhCong BIT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @TongTienSauGiam DECIMAL(12,2);
    DECLARE @TienGiam DECIMAL(12,2);
    DECLARE @TyLeGiam DECIMAL(5,2);
    DECLARE @TienThanhToan DECIMAL(12,2);
    DECLARE @KhachHangID INT;
    DECLARE @DiemID INT;
    DECLARE @DiemKhaDung INT;
    DECLARE @ErrorCode INT = 0;
    
    BEGIN TRANSACTION;
    
    -- Lấy thông tin khách hàng
    SELECT @KhachHangID = dp.KhachHangID
    FROM HoaDon hd
    INNER JOIN DatPhong dp ON hd.DatPhongID = dp.DatPhongID
    WHERE hd.HoaDonID = @HoaDonID;
    
    -- Lấy điểm khả dụng
    SELECT @DiemID = DiemID, @DiemKhaDung = TongDiem - DiemDaSuDung
    FROM DiemThanhVien
    WHERE KhachHangID = @KhachHangID;
    
    -- Kiểm tra điểm sử dụng
    IF @SoDiemSuDung > @DiemKhaDung
    BEGIN
        SET @ThanhCong = 0;
        ROLLBACK TRANSACTION;
        RETURN;
    END
    
    -- Tính tổng tiền sau giảm
    EXEC sp_TinhTongTienHoaDon @HoaDonID, @TongTienSauGiam OUTPUT, @TienGiam OUTPUT, @TyLeGiam OUTPUT;
    
    -- Tính tiền sau khi sử dụng điểm (1 điểm = 10,000 VND)
    SET @TienThanhToan = @TongTienSauGiam - (@SoDiemSuDung * 10000);
    IF @TienThanhToan < 0 SET @TienThanhToan = 0;
    
    -- Cập nhật trạng thái hóa đơn
    UPDATE HoaDon 
    SET TrangThai = 'DaThanhToan',
        TongTien = @TienThanhToan
    WHERE HoaDonID = @HoaDonID;
    
    -- Ghi nhận sử dụng điểm nếu có
    IF @SoDiemSuDung > 0
    BEGIN
        INSERT INTO LichSuDiem (DiemID, SoDiemThayDoi, LoaiGiaoDich, MoTa, ThoiGian)
        VALUES (@DiemID, @SoDiemSuDung, 'SuDungDiem', 
               'Sử dụng điểm để giảm giá hóa đơn #' + CAST(@HoaDonID AS NVARCHAR(10)), GETDATE());
    END
    
    IF @@ERROR <> 0
    BEGIN
        SET @ErrorCode = @@ERROR;
        ROLLBACK TRANSACTION;
        SET @ThanhCong = 0;
        RETURN @ErrorCode;
    END
    
    SET @ThanhCong = 1;
    COMMIT TRANSACTION;
END;
GO

-- Stored Procedure 8: Báo cáo doanh thu chi tiết
CREATE OR ALTER PROCEDURE sp_BaoCaoDoanhThuChiTiet
    @TuNgay DATETIME = NULL,
    @DenNgay DATETIME = NULL,
    @LoaiPhong NVARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    IF @TuNgay IS NULL SET @TuNgay = DATEADD(DAY, -30, GETDATE());
    IF @DenNgay IS NULL SET @DenNgay = GETDATE();
    
    -- Tổng quan doanh thu
    SELECT 
        'TongQuan' as LoaiBaoCao,
        COUNT(DISTINCT hd.HoaDonID) as TongSoHoaDon,
        SUM(hd.TongTien) as TongDoanhThu,
        AVG(hd.TongTien) as DoanhThuTrungBinh,
        COUNT(DISTINCT dp.KhachHangID) as TongSoKhachHang,
        SUM(DATEDIFF(MINUTE, dp.GioBatDau, dp.GioKetThuc)) / 60.0 as TongSoGioSuDung
    FROM HoaDon hd
    INNER JOIN DatPhong dp ON hd.DatPhongID = dp.DatPhongID
    INNER JOIN Phong p ON dp.PhongID = p.PhongID
    WHERE hd.NgayLap BETWEEN @TuNgay AND @DenNgay
    AND hd.TrangThai = 'DaThanhToan'
    AND (@LoaiPhong IS NULL OR p.LoaiPhong = @LoaiPhong);
    
    -- Doanh thu theo loại phòng
    SELECT 
        p.LoaiPhong,
        COUNT(DISTINCT hd.HoaDonID) as TongSoHoaDon,
        SUM(hd.TongTien) as TongDoanhThu,
        AVG(hd.TongTien) as DoanhThuTrungBinh,
        COUNT(DISTINCT dp.KhachHangID) as TongSoKhachHang,
        SUM(DATEDIFF(MINUTE, dp.GioBatDau, dp.GioKetThuc)) / 60.0 as TongSoGioSuDung
    FROM HoaDon hd
    INNER JOIN DatPhong dp ON hd.DatPhongID = dp.DatPhongID
    INNER JOIN Phong p ON dp.PhongID = p.PhongID
    WHERE hd.NgayLap BETWEEN @TuNgay AND @DenNgay
    AND hd.TrangThai = 'DaThanhToan'
    AND (@LoaiPhong IS NULL OR p.LoaiPhong = @LoaiPhong)
    GROUP BY p.LoaiPhong;
    
    -- Top 10 khách hàng thân thiết
    SELECT TOP 10
        kh.KhachHangID,
        kh.HoTen,
        COUNT(DISTINCT hd.HoaDonID) as SoLanSuDung,
        SUM(hd.TongTien) as TongChiTieu,
        AVG(hd.TongTien) as ChiTieuTrungBinh,
        dt.TongDiem as TongDiemHienTai
    FROM HoaDon hd
    INNER JOIN DatPhong dp ON hd.DatPhongID = dp.DatPhongID
    INNER JOIN KhachHang kh ON dp.KhachHangID = kh.KhachHangID
    INNER JOIN DiemThanhVien dt ON kh.KhachHangID = dt.KhachHangID
    WHERE hd.NgayLap BETWEEN @TuNgay AND @DenNgay
    AND hd.TrangThai = 'DaThanhToan'
    GROUP BY kh.KhachHangID, kh.HoTen, dt.TongDiem
    ORDER BY SUM(hd.TongTien) DESC;
END;
GO