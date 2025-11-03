-- Trigger 1: Tự động tạo DiemThanhVien khi có KhachHang mới
CREATE OR ALTER TRIGGER TR_KhachHang_Insert_DiemThanhVien
ON KhachHang
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    
    INSERT INTO DiemThanhVien (KhachHangID, TongDiem, DiemDaSuDung, NgayCapNhat)
    SELECT KhachHangID, 0, 0, GETDATE()
    FROM inserted;
END;
GO

-- Trigger 2: Cập nhật trạng thái phòng khi có đặt phòng
CREATE OR ALTER TRIGGER TR_DatPhong_Update_TrangThaiPhong
ON DatPhong
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE p
    SET p.TrangThai = 
        CASE 
            WHEN i.TrangThai IN ('ChoXacNhan', 'DaXacNhan') THEN 'DaDat'
            WHEN i.TrangThai = 'DangSuDung' THEN 'DangSuDung'
            WHEN i.TrangThai IN ('DaHuy', 'HoanTat') THEN 'Trong'
            ELSE p.TrangThai
        END
    FROM Phong p
    INNER JOIN inserted i ON p.PhongID = i.PhongID;
END;
GO

-- Trigger 3: Tự động tính điểm thành viên khi thanh toán hóa đơn
CREATE OR ALTER TRIGGER TR_HoaDon_Update_TinhDiem
ON HoaDon
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    
    IF UPDATE(TrangThai)
    BEGIN
        -- Khi hóa đơn chuyển sang trạng thái đã thanh toán
        INSERT INTO LichSuDiem (DiemID, SoDiemThayDoi, LoaiGiaoDich, MoTa, ThoiGian)
        SELECT 
            dt.DiemID,
            CASE 
                WHEN p.LoaiPhong = 'VIP' THEN CAST((i.TongTien / 8000) AS INT)
                WHEN p.LoaiPhong = 'Thuong' THEN CAST((i.TongTien / 10000) AS INT)
                ELSE CAST((i.TongTien / 10000) AS INT)
            END AS SoDiem,
            'TichDiem',
            'Tích điểm từ hóa đơn #' + CAST(i.HoaDonID AS NVARCHAR(10)) + ' - Phòng ' + p.LoaiPhong,
            GETDATE()
        FROM inserted i
        INNER JOIN DatPhong dp ON i.DatPhongID = dp.DatPhongID
        INNER JOIN Phong p ON dp.PhongID = p.PhongID
        INNER JOIN DiemThanhVien dt ON dp.KhachHangID = dt.KhachHangID
        WHERE i.TrangThai = 'DaThanhToan'
        AND EXISTS (SELECT 1 FROM deleted d WHERE d.HoaDonID = i.HoaDonID AND d.TrangThai != 'DaThanhToan');

        -- Cập nhật tổng điểm
        UPDATE dt
        SET dt.TongDiem = dt.TongDiem + 
            CASE 
                WHEN p.LoaiPhong = 'VIP' THEN CAST((i.TongTien / 8000) AS INT)
                WHEN p.LoaiPhong = 'Thuong' THEN CAST((i.TongTien / 10000) AS INT)
                ELSE CAST((i.TongTien / 10000) AS INT)
            END,
            dt.NgayCapNhat = GETDATE()
        FROM DiemThanhVien dt
        INNER JOIN DatPhong dp ON dt.KhachHangID = dp.KhachHangID
        INNER JOIN inserted i ON dp.DatPhongID = i.DatPhongID
        INNER JOIN Phong p ON dp.PhongID = p.PhongID
        WHERE i.TrangThai = 'DaThanhToan'
        AND EXISTS (SELECT 1 FROM deleted d WHERE d.HoaDonID = i.HoaDonID AND d.TrangThai != 'DaThanhToan');
    END
END;
GO

-- Trigger 4: Tự động tính tổng tiền hóa đơn khi thêm/sửa chi tiết hóa đơn
CREATE OR ALTER TRIGGER TR_ChiTietHoaDon_Update_TongTien
ON ChiTietHoaDon
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Cập nhật tổng tiền cho các hóa đơn bị ảnh hưởng
    UPDATE hd
    SET hd.TongTien = (
        SELECT ISNULL(SUM(ct.SoLuong * ct.DonGia), 0)
        FROM ChiTietHoaDon ct
        WHERE ct.HoaDonID = hd.HoaDonID
    )
    FROM HoaDon hd
    WHERE hd.HoaDonID IN (
        SELECT HoaDonID FROM inserted
        UNION
        SELECT HoaDonID FROM deleted
    );
END;
GO

-- Trigger 5: Kiểm tra tính khả dụng của phòng khi đặt phòng (SỬA THÀNH AFTER TRIGGER)
CREATE OR ALTER TRIGGER TR_DatPhong_Check_PhongTrong
ON DatPhong
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @IsValid BIT = 1;
    DECLARE @ErrorMessage NVARCHAR(500) = '';

    -- Kiểm tra xem phòng có bị trùng giờ không
    IF EXISTS (
        SELECT 1 FROM inserted i
        INNER JOIN DatPhong dp ON i.PhongID = dp.PhongID
        WHERE dp.TrangThai NOT IN ('DaHuy', 'HoanTat')
        AND dp.DatPhongID != i.DatPhongID
        AND i.GioBatDau < dp.GioKetThuc 
        AND i.GioKetThuc > dp.GioBatDau
    )
    BEGIN
        SET @IsValid = 0;
        SET @ErrorMessage = 'Phòng không khả dụng trong khoảng thời gian này.';
    END

    -- Kiểm tra xem phòng có đang trống không
    IF EXISTS (
        SELECT 1 FROM inserted i
        INNER JOIN Phong p ON i.PhongID = p.PhongID
        WHERE p.TrangThai != 'Trong'
    )
    BEGIN
        SET @IsValid = 0;
        SET @ErrorMessage = 'Phòng hiện không trống.';
    END

    IF @IsValid = 0
    BEGIN
        -- Rollback transaction và hủy đặt phòng
        ROLLBACK TRANSACTION;
        RAISERROR(@ErrorMessage, 16, 1);
    END
END;
GO

-- Trigger 6: Tự động cập nhật giá giờ khi thay đổi loại phòng
CREATE OR ALTER TRIGGER TR_Phong_Update_GiaTheoLoai
ON Phong
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    
    IF UPDATE(LoaiPhong)
    BEGIN
        UPDATE p
        SET p.GiaGio = 
            CASE 
                WHEN i.LoaiPhong = 'VIP' THEN 200000
                WHEN i.LoaiPhong = 'Thuong' THEN 150000
                ELSE p.GiaGio
            END
        FROM Phong p
        INNER JOIN inserted i ON p.PhongID = i.PhongID
        WHERE i.LoaiPhong != COALESCE((SELECT LoaiPhong FROM deleted WHERE PhongID = i.PhongID), '');
    END
END;
GO

-- Trigger 7: Tính tiền phòng theo loại phòng khi tạo hóa đơn
CREATE OR ALTER TRIGGER TR_HoaDon_Insert_TinhTienPhong
ON HoaDon
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @HoaDonID INT, @DatPhongID INT, @TienPhong DECIMAL(12,2);
    
    DECLARE hoaDon_cursor CURSOR FOR
    SELECT HoaDonID, DatPhongID FROM inserted;
    
    OPEN hoaDon_cursor;
    FETCH NEXT FROM hoaDon_cursor INTO @HoaDonID, @DatPhongID;
    
    WHILE @@FETCH_STATUS = 0
    BEGIN
        -- Tính tiền phòng dựa trên loại phòng và thời gian sử dụng
        EXEC sp_TinhTienPhongChiTiet @DatPhongID, @TienPhong OUTPUT;
        
        -- Cập nhật tổng tiền hóa đơn (chỉ tiền phòng)
        UPDATE HoaDon 
        SET TongTien = @TienPhong
        WHERE HoaDonID = @HoaDonID;
        
        FETCH NEXT FROM hoaDon_cursor INTO @HoaDonID, @DatPhongID;
    END
    
    CLOSE hoaDon_cursor;
    DEALLOCATE hoaDon_cursor;
END;
GO

-- Trigger 8: Tự động áp dụng khuyến mãi theo loại phòng
CREATE OR ALTER TRIGGER TR_HoaDon_ApDungKhuyenMaiAuto
ON HoaDon
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE hd
    SET hd.KM_ID = km.KM_ID
    FROM HoaDon hd
    INNER JOIN inserted i ON hd.HoaDonID = i.HoaDonID
    INNER JOIN DatPhong dp ON hd.DatPhongID = dp.DatPhongID
    INNER JOIN Phong p ON dp.PhongID = p.PhongID
    INNER JOIN KhuyenMai km ON (
        -- Áp dụng khuyến mãi tự động theo loại phòng
        (p.LoaiPhong = 'VIP' AND km.MaKM LIKE '%VIP%') OR
        (p.LoaiPhong = 'Thuong' AND km.MaKM LIKE '%WELCOME%')
    )
    WHERE km.NgayBatDau <= GETDATE() AND km.NgayKetThuc >= GETDATE()
    AND hd.KM_ID IS NULL;
END;
GO

-- Trigger 9: Thống kê phòng theo loại khi có thay đổi
CREATE OR ALTER TRIGGER TR_Phong_ThongKeTheoLoai
ON Phong
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Cập nhật thống kê khi có thay đổi trạng thái phòng
    IF UPDATE(TrangThai) OR UPDATE(LoaiPhong) OR EXISTS (SELECT 1 FROM deleted)
    BEGIN
        IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'PhongThongKe')
        BEGIN
            CREATE TABLE PhongThongKe (
                ThongKeID INT IDENTITY(1,1) PRIMARY KEY,
                LoaiPhong NVARCHAR(50) NOT NULL,
                TongSoPhong INT NOT NULL,
                SoLuongTrong INT NOT NULL,
                SoLuongDaDat INT NOT NULL,
                SoLuongDangSuDung INT NOT NULL,
                ThoiGianCapNhat DATETIME NOT NULL DEFAULT GETDATE()
            );
        END
        
        INSERT INTO PhongThongKe (LoaiPhong, TongSoPhong, SoLuongTrong, SoLuongDaDat, SoLuongDangSuDung, ThoiGianCapNhat)
        SELECT 
            p.LoaiPhong,
            COUNT(*) as TongSoPhong,
            COUNT(CASE WHEN p.TrangThai = 'Trong' THEN 1 END) as SoLuongTrong,
            COUNT(CASE WHEN p.TrangThai = 'DaDat' THEN 1 END) as SoLuongDaDat,
            COUNT(CASE WHEN p.TrangThai = 'DangSuDung' THEN 1 END) as SoLuongDangSuDung,
            GETDATE()
        FROM Phong p
        GROUP BY p.LoaiPhong;
    END
END;
GO

-- Trigger 10: Kiểm tra và cập nhật khi sử dụng điểm thành viên (SỬA THÀNH AFTER TRIGGER)
CREATE OR ALTER TRIGGER TR_LichSuDiem_Check_Diem
ON LichSuDiem
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @DiemID INT, @SoDiemThayDoi INT, @LoaiGiaoDich NVARCHAR(50);
    DECLARE @TongDiemHienTai INT, @DiemKhaDung INT;
    
    DECLARE diem_cursor CURSOR FOR
    SELECT DiemID, SoDiemThayDoi, LoaiGiaoDich FROM inserted;
    
    OPEN diem_cursor;
    FETCH NEXT FROM diem_cursor INTO @DiemID, @SoDiemThayDoi, @LoaiGiaoDich;
    
    WHILE @@FETCH_STATUS = 0
    BEGIN
        -- Lấy tổng điểm hiện tại
        SELECT @TongDiemHienTai = TongDiem, @DiemKhaDung = TongDiem - DiemDaSuDung
        FROM DiemThanhVien
        WHERE DiemID = @DiemID;
        
        -- Kiểm tra nếu là sử dụng điểm và điểm không đủ
        IF @LoaiGiaoDich = 'SuDungDiem' AND @SoDiemThayDoi > @DiemKhaDung
        BEGIN
            ROLLBACK TRANSACTION;
            RAISERROR('Số điểm sử dụng vượt quá điểm khả dụng. Điểm khả dụng: %d', 16, 1, @DiemKhaDung);
            CLOSE diem_cursor;
            DEALLOCATE diem_cursor;
            RETURN;
        END
        
        -- Cập nhật DiemThanhVien
        IF @LoaiGiaoDich = 'SuDungDiem'
        BEGIN
            UPDATE DiemThanhVien
            SET DiemDaSuDung = DiemDaSuDung + @SoDiemThayDoi,
                NgayCapNhat = GETDATE()
            WHERE DiemID = @DiemID;
        END
        
        FETCH NEXT FROM diem_cursor INTO @DiemID, @SoDiemThayDoi, @LoaiGiaoDich;
    END
    
    CLOSE diem_cursor;
    DEALLOCATE diem_cursor;
END;
GO