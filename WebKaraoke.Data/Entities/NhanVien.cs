using System.ComponentModel.DataAnnotations.Schema;

namespace WebKaraoke.Data.Entities
{
    [Table("NhanVien")]
    public class NhanVien
    {
        public int NhanVienID { get; set; }
        public string HoTen { get; set; } = string.Empty;
        public string SoDienThoai { get; set; } = string.Empty;
        public string ChucVu { get; set; } = string.Empty;

        // Navigation properties
        public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();
        public virtual TaiKhoan? TaiKhoan { get; set; }
    }
}