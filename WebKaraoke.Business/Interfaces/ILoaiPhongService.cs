// WebKaraoke.Business/Interfaces/ILoaiPhongService.cs
using WebKaraoke.DTO;

namespace WebKaraoke.Business.Interfaces
{
    public interface ILoaiPhongService
    {
        Task<IEnumerable<LoaiPhongDTO>> GetAllLoaiPhongsAsync();
        Task<LoaiPhongDTO?> GetLoaiPhongByIdAsync(int id);
        Task<bool> CreateLoaiPhongAsync(LoaiPhongCreateDTO loaiPhong);
        Task<bool> UpdateLoaiPhongAsync(int id, LoaiPhongUpdateDTO loaiPhong);
        Task<bool> DeleteLoaiPhongAsync(int id);
        Task<IEnumerable<LoaiPhongDTO>> SearchLoaiPhongAsync(string tenLoai);
    }
}