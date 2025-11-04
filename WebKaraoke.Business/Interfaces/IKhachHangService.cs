using WebKaraoke.DTO;

namespace WebKaraoke.Business.Interfaces
{
    public interface IKhachHangService
    {
        Task<KhachHangDTO?> GetKhachHangByIdAsync(int id);
        Task<bool> UpdateKhachHangAsync(int id, KhachHangUpdateDTO khachHang);
        Task<IEnumerable<KhachHangDTO>> GetAllKhachHangsAsync();
        Task<DiemThanhVienDTO?> GetDiemThanhVienAsync(int khachHangId);
    }
}