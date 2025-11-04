namespace WebKaraoke.DTO
{
    public class ThongKeDoanhThuDTO
    {
        public DateTime TuNgay { get; set; }
        public DateTime DenNgay { get; set; }
        public decimal TongDoanhThu { get; set; }
        public int TongHoaDon { get; set; }
        public int TongKhachHang { get; set; }
        public decimal DoanhThuTrungBinh => TongHoaDon > 0 ? TongDoanhThu / TongHoaDon : 0;
        
        public List<DoanhThuTheoNgayDTO> DoanhThuTheoNgay { get; set; } = new();
        public List<DoanhThuTheoPhongDTO> DoanhThuTheoPhong { get; set; } = new();
        public List<MonBanChayDTO> MonBanChay { get; set; } = new();
    }

    public class DoanhThuTheoNgayDTO
    {
        public DateTime Ngay { get; set; }
        public decimal DoanhThu { get; set; }
        public int SoHoaDon { get; set; }
    }

    public class DoanhThuTheoPhongDTO
    {
        public string TenPhong { get; set; } = string.Empty;
        public string LoaiPhong { get; set; } = string.Empty;
        public decimal DoanhThu { get; set; }
        public int SoLanSuDung { get; set; }
    }

    public class MonBanChayDTO
    {
        public int MonID { get; set; }
        public string TenMon { get; set; } = string.Empty;
        public string DanhMuc { get; set; } = string.Empty;
        public int SoLuongBan { get; set; }
        public decimal DoanhThu { get; set; }
    }

    public class ThongKeRequest
    {
        public DateTime TuNgay { get; set; }
        public DateTime DenNgay { get; set; }
        public string? LoaiThongKe { get; set; } // "ngay", "thang", "nam"
    }
}