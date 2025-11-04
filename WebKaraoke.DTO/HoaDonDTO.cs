namespace WebKaraoke.DTO
{
    public class HoaDonDTO
    {
        public int HoaDonID { get; set; }
        public int DatPhongID { get; set; }
        public int NhanVienID { get; set; }
        public int? KM_ID { get; set; }
        public DateTime NgayLap { get; set; }
        public decimal TongTien { get; set; }
        public string TrangThai { get; set; } = string.Empty;
        
        // Additional info
        public string TenNhanVien { get; set; } = string.Empty;
        public string MaKhuyenMai { get; set; } = string.Empty;
        public decimal TyLeGiam { get; set; }
        public decimal TienGiam { get; set; }
        public decimal ThanhTien { get; set; }
        
        public List<ChiTietHoaDonDTO> ChiTietHoaDons { get; set; } = new();
    }

    public class HoaDonCreateDTO
    {
        public int DatPhongID { get; set; }
        public int NhanVienID { get; set; }
        public int? KM_ID { get; set; }
    }

    public class ChiTietHoaDonDTO
    {
        public int CTHD_ID { get; set; }
        public int HoaDonID { get; set; }
        public int MonID { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public decimal ThanhTien => SoLuong * DonGia;
        
        // Additional info
        public string TenMon { get; set; } = string.Empty;
        public string DanhMuc { get; set; } = string.Empty;
    }

    public class ThemMonVaoHoaDonRequest
    {
        public int MonID { get; set; }
        public int SoLuong { get; set; }
    }

    public class ApDungKhuyenMaiRequest
    {
        public string MaKhuyenMai { get; set; } = string.Empty;
    }
}