using System.ComponentModel.DataAnnotations.Schema;

namespace WebKaraoke.Data.Entities
{
    [Table("KhuyenMai")]
    public class KhuyenMai
    {
        public int KM_ID { get; set; }
        public string MaKM { get; set; } = string.Empty;
        public decimal TyLeGiam { get; set; } // Ví dụ 10.00 = 10%
        public DateTime NgayBatDau { get; set; }
        public DateTime NgayKetThuc { get; set; }
        public string? MoTa { get; set; }

        // Navigation properties
        public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();
    }
}