// WebKaraoke.DTO/PhongDTO.cs
namespace WebKaraoke.DTO
{
    public class PhongDTO
    {
        public int PhongID { get; set; }
        public string TenPhong { get; set; } = string.Empty;
        public string LoaiPhong { get; set; } = string.Empty; // Giữ nguyên là string
        public decimal GiaGio { get; set; }
        public string TrangThai { get; set; } = string.Empty;
    }

    // Các DTO khác...
}