using System.ComponentModel.DataAnnotations.Schema;

namespace WebKaraoke.Data.Entities
{

    [Table("MonAnNuocUong")]
    public class MonAnNuocUong
    {
        public int MonID { get; set; }
        public string TenMon { get; set; } = string.Empty;
        public decimal DonGia { get; set; }
        public string DanhMuc { get; set; } = string.Empty;
        public string? MoTa { get; set; }
        public string? HinhAnh { get; set; }
        public bool DangKinhDoanh { get; set; } = true;
        
        // Thêm field hình ảnh (URL)
        public string? HinhAnhUrl { get; set; }

        // Navigation properties
        public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; } = new List<ChiTietHoaDon>();
    }
}