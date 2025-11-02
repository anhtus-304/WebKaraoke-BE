using WebKaraoke.DTO;
using Microsoft.Extensions.Logging;
namespace WebKaraoke.Business.Interfaces
{
    public interface IDatPhongService
    {
        Task<IEnumerable<DatPhongDTO>> GetDatPhongByKhachHangAsync(int khachHangId);
        Task<DatPhongDTO?> GetDatPhongByIdAsync(int id);
        Task<bool> HuyDatPhongAsync(int datPhongId, int khachHangId);
        Task<IEnumerable<DatPhongDTO>> GetDatPhongChoXacNhanAsync();
        Task<bool> XacNhanDatPhongAsync(int datPhongId, int nhanVienId);
    }
}