namespace WebKaraoke.DTO
{
    public class PhongDTO
    {
        public int PhongID { get; set; }
        public string TenPhong { get; set; } = string.Empty;
        public int LoaiPhongID { get; set; } // THÊM: LoaiPhongID
        public string TenLoaiPhong { get; set; } = string.Empty; // THÊM: Tên loại phòng
        public decimal GiaGio { get; set; }
        public int SucChua { get; set; } // THÊM: Sức chứa
        public string TrangThai { get; set; } = string.Empty;
    }

    public class PhongCreateDTO
    {
        public string TenPhong { get; set; } = string.Empty;
        public int LoaiPhongID { get; set; } // SỬA: Thành LoaiPhongID (int)
        public string TrangThai { get; set; } = "Trong";
        // XÓA: GiaGio vì đã nằm trong LoaiPhong
    }

    public class PhongUpdateDTO
    {
        public string TenPhong { get; set; } = string.Empty;
        public int LoaiPhongID { get; set; } // SỬA: Thành LoaiPhongID (int)
        public string TrangThai { get; set; } = string.Empty;
        // XÓA: GiaGio vì đã nằm trong LoaiPhong
    }
}