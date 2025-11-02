using System.ComponentModel.DataAnnotations.Schema;

namespace WebKaraoke.Data.Entities
{
    [Table("Phong")] // THÊM DÒNG NÀY
    public class Phong
    {
        public int PhongID { get; set; }
        public string TenPhong { get; set; } = string.Empty;
        public string LoaiPhong { get; set; } = string.Empty; // VIP, Thuong
        public decimal GiaGio { get; set; }
        public string TrangThai { get; set; } = "Trong"; // Trong, DaDat, DangSuDung, DangDon
        
        // Navigation properties
        public virtual ICollection<DatPhong> DatPhongs { get; set; } = new List<DatPhong>();
    }
}