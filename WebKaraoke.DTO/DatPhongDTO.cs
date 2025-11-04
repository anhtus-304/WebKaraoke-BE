namespace WebKaraoke.DTO
{
    public class DatPhongDTO
    {
        public int DatPhongID { get; set; }
        public int KhachHangID { get; set; }
        public int PhongID { get; set; }
        public DateTime ThoiGianDat { get; set; }
        public DateTime GioBatDau { get; set; }
        public DateTime GioKetThuc { get; set; }
        public int SoLuongNguoi { get; set; }
        public string TrangThai { get; set; } = string.Empty;
        
        // Additional info for display
        public string TenPhong { get; set; } = string.Empty;
        public string TenKhachHang { get; set; } = string.Empty;
        public string LoaiPhong { get; set; } = string.Empty;
        public decimal GiaGio { get; set; }
    }

    public class DatPhongRequest
    {
        public int PhongID { get; set; }
        public DateTime GioBatDau { get; set; }
        public DateTime GioKetThuc { get; set; }
        public int SoLuongNguoi { get; set; }
    }

    public class DatPhongUpdateDTO
    {
        public DateTime GioBatDau { get; set; }
        public DateTime GioKetThuc { get; set; }
        public int SoLuongNguoi { get; set; }
        public string TrangThai { get; set; } = string.Empty;
    }
}