using System.ComponentModel.DataAnnotations.Schema;

namespace WebKaraoke.Data.Entities
{
    [Table("HoaDon")]
    public class HoaDon
    {
        public int HoaDonID { get; set; }
        public int DatPhongID { get; set; }
        public int NhanVienID { get; set; }
        public int? KM_ID { get; set; }
        public DateTime NgayLap { get; set; }
        public decimal TongTien { get; set; }
        public string TrangThai { get; set; } = "ChuaThanhToan"; // ChuaThanhToan, DaThanhToan

        // Navigation properties
        public virtual DatPhong DatPhong { get; set; } = null!;
        public virtual NhanVien NhanVien { get; set; } = null!;
        public virtual KhuyenMai? KhuyenMai { get; set; }
        public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; } = new List<ChiTietHoaDon>();
    }
}