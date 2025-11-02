using System.ComponentModel.DataAnnotations.Schema;

namespace WebKaraoke.Data.Entities
{
    [Table("DatPhong")]
    public class DatPhong
    {
        public int DatPhongID { get; set; }
        public int KhachHangID { get; set; }
        public int PhongID { get; set; }
        public DateTime ThoiGianDat { get; set; }
        public DateTime GioBatDau { get; set; }
        public DateTime GioKetThuc { get; set; }
        public int SoLuongNguoi { get; set; }
        public string TrangThai { get; set; } = "ChoXacNhan"; // ChoXacNhan, DaXacNhan, DaHuy, DangSuDung, HoanTat

        // Navigation properties
        public virtual KhachHang KhachHang { get; set; } = null!;
        public virtual Phong Phong { get; set; } = null!;
        public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>(); // Thêm dòng này
    }
}