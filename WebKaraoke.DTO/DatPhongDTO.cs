// WebKaraoke.DTO/DatPhongDTO.cs
namespace WebKaraoke.DTO
{
    public class DatPhongDTO
    {
        public int DatPhongID { get; set; }
        public string TenPhong { get; set; } = string.Empty;
        public string TenKhachHang { get; set; } = string.Empty;
        public string LoaiPhong { get; set; } = string.Empty;
        public decimal GiaGio { get; set; }
        public DateTime NgayDat { get; set; } // THÊM PROPERTY NÀY
        public DateTime GioBatDau { get; set; }
        public DateTime GioKetThuc { get; set; }
        public string TrangThai { get; set; } = string.Empty;
    }

    public class DatPhongRequest
    {
        public int PhongID { get; set; }
        public DateTime GioBatDau { get; set; }
        public DateTime GioKetThuc { get; set; }
    }

    public class DatPhongUpdateDTO
    {
        public string TrangThai { get; set; } = string.Empty;
    }
}