using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebKaraoke.Data.Entities
{
    [Table("Phong")]
    public class Phong
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PhongID { get; set; }

        [Required]
        [StringLength(100)]
        public string TenPhong { get; set; } = string.Empty;

        // Replace the string LoaiPhong with foreign key
        public int LoaiPhongID { get; set; }

        [ForeignKey("LoaiPhongID")]
        public virtual LoaiPhong LoaiPhong { get; set; } = null!;

        // Remove GiaGio since it's now in LoaiPhong
        // public decimal GiaGio { get; set; }

        [Required]
        [StringLength(20)]
        public string TrangThai { get; set; } = "Trong";

        // Navigation properties
        public virtual ICollection<DatPhong> DatPhongs { get; set; } = new List<DatPhong>();
    }
}