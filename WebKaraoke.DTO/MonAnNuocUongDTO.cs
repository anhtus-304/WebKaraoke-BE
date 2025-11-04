namespace WebKaraoke.DTO
{
    public class MonAnNuocUongDTO
    {
        public int MonID { get; set; }
        public string TenMon { get; set; } = string.Empty;
        public decimal DonGia { get; set; }
        public string DanhMuc { get; set; } = string.Empty;
        public string? MoTa { get; set; }
        public string? HinhAnh { get; set; }
        public bool DangKinhDoanh { get; set; }
    }

    public class MonAnNuocUongCreateDTO
    {
        public string TenMon { get; set; } = string.Empty;
        public decimal DonGia { get; set; }
        public string DanhMuc { get; set; } = string.Empty;
        public string? MoTa { get; set; }
        public string? HinhAnh { get; set; }
        public bool DangKinhDoanh { get; set; } = true;
    }

    public class MonAnNuocUongUpdateDTO
    {
        public string TenMon { get; set; } = string.Empty;
        public decimal DonGia { get; set; }
        public string DanhMuc { get; set; } = string.Empty;
        public string? MoTa { get; set; }
        public string? HinhAnh { get; set; }
        public bool DangKinhDoanh { get; set; }
    }
}