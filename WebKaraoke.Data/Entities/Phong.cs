// WebKaraoke.Data/Entities/Phong.cs
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

        // THAY THẾ property LoaiPhong cũ bằng LoaiPhongID
        public int LoaiPhongID { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal GiaGio { get; set; }

        [Required]
        [StringLength(20)]
        public string TrangThai { get; set; } = "Trong";

        // Navigation properties
        [ForeignKey("LoaiPhongID")]
        public virtual LoaiPhong LoaiPhong { get; set; } = null!;
        
        public virtual ICollection<DatPhong> DatPhongs { get; set; } = new List<DatPhong>();
    }
}