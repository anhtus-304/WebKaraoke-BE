using System.ComponentModel.DataAnnotations.Schema;

namespace WebKaraoke.Data.Entities
{
    [Table("TaiKhoan")] // Thêm dòng này
    public class TaiKhoan
    {
        public int TaiKhoanID { get; set; }
        public string Username { get; set; } = string.Empty;
        public string MatKhauHash { get; set; } = string.Empty;
        public string Role { get; set; } = "Khach"; // Khach, NhanVien, Admin
        public int? KhachHangID { get; set; }
        public int? NhanVienID { get; set; }
        
        // Navigation properties
        public virtual KhachHang? KhachHang { get; set; }
        public virtual NhanVien? NhanVien { get; set; }
    }
}