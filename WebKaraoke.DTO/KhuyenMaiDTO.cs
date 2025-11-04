namespace WebKaraoke.DTO
{
    public class KhuyenMaiDTO
    {
        public int KM_ID { get; set; }
        public string MaKM { get; set; } = string.Empty;
        public decimal TyLeGiam { get; set; }
        public DateTime NgayBatDau { get; set; }
        public DateTime NgayKetThuc { get; set; }
        public string? MoTa { get; set; }
        public bool IsActive => DateTime.Now >= NgayBatDau && DateTime.Now <= NgayKetThuc;
    }

    public class KhuyenMaiCreateDTO
    {
        public string MaKM { get; set; } = string.Empty;
        public decimal TyLeGiam { get; set; }
        public DateTime NgayBatDau { get; set; }
        public DateTime NgayKetThuc { get; set; }
        public string? MoTa { get; set; }
    }

    public class KhuyenMaiUpdateDTO
    {
        public string MaKM { get; set; } = string.Empty;
        public decimal TyLeGiam { get; set; }
        public DateTime NgayBatDau { get; set; }
        public DateTime NgayKetThuc { get; set; }
        public string? MoTa { get; set; }
    }
}