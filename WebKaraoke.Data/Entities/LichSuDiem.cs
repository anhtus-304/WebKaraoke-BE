using System.ComponentModel.DataAnnotations.Schema;

namespace WebKaraoke.Data.Entities
{
    [Table("LichSuDiem")]
    public class LichSuDiem
    {
        public int LichSuID { get; set; }
        public int DiemID { get; set; }
        public int SoDiemThayDoi { get; set; }
        public string LoaiGiaoDich { get; set; } = string.Empty; // TichDiem, SuDungDiem
        public string MoTa { get; set; } = string.Empty;
        public DateTime ThoiGian { get; set; }

        // Navigation properties
        public virtual DiemThanhVien DiemThanhVien { get; set; } = null!;
    }
}