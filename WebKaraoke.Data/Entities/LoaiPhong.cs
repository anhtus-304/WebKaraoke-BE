// WebKaraoke.Data/Entities/LoaiPhong.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebKaraoke.Data.Entities
{
    [Table("LoaiPhong")]
    public class LoaiPhong
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LoaiPhongID { get; set; }

        [Required]
        [StringLength(100)]
        public string TenLoai { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal GiaPhong { get; set; }

        [Required]
        public int SucChua { get; set; }

        [StringLength(500)]
        public string? MoTa { get; set; }

        // Navigation property
        public virtual ICollection<Phong> Phongs { get; set; } = new List<Phong>();
    }
}