namespace WebKaraoke.DTO
{
    public class DiemThanhVienDTO
    {
        public int DiemID { get; set; }
        public int KhachHangID { get; set; }
        public int TongDiem { get; set; }
        public int DiemDaSuDung { get; set; }
        public int DiemKhaDung => TongDiem - DiemDaSuDung;
        public DateTime NgayCapNhat { get; set; }
        
        // Additional info
        public string TenKhachHang { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }

    public class LichSuDiemDTO
    {
        public int LichSuID { get; set; }
        public int DiemID { get; set; }
        public int SoDiemThayDoi { get; set; }
        public string LoaiGiaoDich { get; set; } = string.Empty;
        public string MoTa { get; set; } = string.Empty;
        public DateTime ThoiGian { get; set; }
    }

    public class TangDiemRequest
    {
        public int KhachHangID { get; set; }
        public int SoDiem { get; set; }
        public string LyDo { get; set; } = string.Empty;
    }

    public class SuDungDiemRequest
    {
        public int KhachHangID { get; set; }
        public int SoDiem { get; set; }
        public string LyDo { get; set; } = string.Empty;
    }
}