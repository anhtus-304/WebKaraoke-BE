using WebKaraoke.DTO;
using Microsoft.Extensions.Logging;
namespace WebKaraoke.Business.Interfaces
{
    public interface IPhongService
    {
        Task<IEnumerable<PhongDTO>> GetAllPhongsAsync();
        Task<PhongDTO?> GetPhongByIdAsync(int id);
        Task<IEnumerable<PhongDTO>> GetAvailablePhongsAsync(DateTime ngay, string? loaiPhong = null);
        Task<bool> DatPhongAsync(DatPhongRequest request, int khachHangId);
        Task<bool> UpdateTrangThaiPhongAsync(int phongId, string trangThai);
    }
}