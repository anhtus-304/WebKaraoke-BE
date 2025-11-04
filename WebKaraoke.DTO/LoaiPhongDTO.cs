// WebKaraoke.DTO/LoaiPhongDTO.cs
namespace WebKaraoke.DTO
{
    public class LoaiPhongDTO
    {
        public int LoaiPhongID { get; set; }
        public string TenLoai { get; set; } = string.Empty;
        public decimal GiaPhong { get; set; }
        public int SucChua { get; set; }
        public string? MoTa { get; set; }
        public int SoLuongPhong { get; set; }
    }

    public class LoaiPhongCreateDTO
    {
        public string TenLoai { get; set; } = string.Empty;
        public decimal GiaPhong { get; set; }
        public int SucChua { get; set; }
        public string? MoTa { get; set; }
    }

    public class LoaiPhongUpdateDTO
    {
        public string TenLoai { get; set; } = string.Empty;
        public decimal GiaPhong { get; set; }
        public int SucChua { get; set; }
        public string? MoTa { get; set; }
    }
}