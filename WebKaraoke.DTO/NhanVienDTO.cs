namespace WebKaraoke.DTO
{
    public class NhanVienDTO
    {
        public int NhanVienID { get; set; }
        public string HoTen { get; set; } = string.Empty;
        public string SoDienThoai { get; set; } = string.Empty;
        public string ChucVu { get; set; } = string.Empty;
    }

    public class NhanVienCreateDTO
    {
        public string HoTen { get; set; } = string.Empty;
        public string SoDienThoai { get; set; } = string.Empty;
        public string ChucVu { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class NhanVienUpdateDTO
    {
        public string HoTen { get; set; } = string.Empty;
        public string SoDienThoai { get; set; } = string.Empty;
        public string ChucVu { get; set; } = string.Empty;
    }
}