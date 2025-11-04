using System.ComponentModel.DataAnnotations.Schema;

namespace WebKaraoke.Data.Entities
{
    [Table("KhachHang")] // THÊM DÒNG NÀY
    public class KhachHang
    {
        public int KhachHangID { get; set; }
        public string HoTen { get; set; } = string.Empty;
        public string? SoDienThoai { get; set; }
        public string? Email { get; set; }
        public string? DiaChi { get; set; }
        public string? MatKhau { get; set; }
        
        // Navigation properties
        public virtual TaiKhoan? TaiKhoan { get; set; }
        public virtual DiemThanhVien? DiemThanhVien { get; set; }
        public virtual ICollection<DatPhong> DatPhongs { get; set; } = new List<DatPhong>();
    }
}