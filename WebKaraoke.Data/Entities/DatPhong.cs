// WebKaraoke.Data/Entities/DatPhong.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebKaraoke.Data.Entities
{
    [Table("DatPhong")]
    public class DatPhong
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DatPhongID { get; set; }

        public int PhongID { get; set; }
        public int KhachHangID { get; set; }

        // KIỂM TRA PROPERTY NÀY TỒN TẠI
        public DateTime NgayDat { get; set; } // Hoặc property khác

        public DateTime GioBatDau { get; set; }
        public DateTime GioKetThuc { get; set; }
        public string TrangThai { get; set; } = "ChoXacNhan";

        // Navigation properties
        [ForeignKey("PhongID")]
        public virtual Phong Phong { get; set; } = null!;
        
        [ForeignKey("KhachHangID")]
        public virtual KhachHang KhachHang { get; set; } = null!;
        
        public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();
    }
}