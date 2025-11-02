using System.ComponentModel.DataAnnotations.Schema;

namespace WebKaraoke.Data.Entities
{
    [Table("ChiTietHoaDon")]
    public class ChiTietHoaDon
    {
        public int CTHD_ID { get; set; }
        public int HoaDonID { get; set; }
        public int MonID { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }

        // Navigation properties
        public virtual HoaDon HoaDon { get; set; } = null!;
        public virtual MonAnNuocUong MonAnNuocUong { get; set; } = null!;
    }
}