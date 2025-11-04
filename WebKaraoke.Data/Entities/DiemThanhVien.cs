using System.ComponentModel.DataAnnotations.Schema;

namespace WebKaraoke.Data.Entities
{
    [Table("DiemThanhViens")]
    public class DiemThanhVien
    {
        public int DiemID { get; set; }
        public int KhachHangID { get; set; }
        public int TongDiem { get; set; }
        public int DiemDaSuDung { get; set; }
        public DateTime NgayCapNhat { get; set; }

        // Navigation properties
        public virtual KhachHang KhachHang { get; set; } = null!;
        public virtual ICollection<LichSuDiem> LichSuDiems { get; set; } = new List<LichSuDiem>();
    }
}