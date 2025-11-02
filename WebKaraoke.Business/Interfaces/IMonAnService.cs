using WebKaraoke.DTO;

namespace WebKaraoke.Business.Interfaces
{
    public interface IMonAnService
    {
        Task<IEnumerable<MonAnNuocUongDTO>> GetAllMonAnAsync();
        Task<IEnumerable<MonAnNuocUongDTO>> GetMonAnByDanhMucAsync(string danhMuc);
        Task<MonAnNuocUongDTO?> GetMonAnByIdAsync(int id);
        Task<bool> AddMonAnAsync(MonAnNuocUongCreateDTO monAn);
        Task<bool> UpdateMonAnAsync(int id, MonAnNuocUongUpdateDTO monAn);
        Task<bool> DeleteMonAnAsync(int id);
    }
}