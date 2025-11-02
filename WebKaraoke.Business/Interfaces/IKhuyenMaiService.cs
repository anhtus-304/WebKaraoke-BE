using WebKaraoke.DTO;
using Microsoft.Extensions.Logging;
namespace WebKaraoke.Business.Interfaces
{
    public interface IKhuyenMaiService
    {
        Task<IEnumerable<KhuyenMaiDTO>> GetAllKhuyenMaiAsync();
        Task<KhuyenMaiDTO?> GetKhuyenMaiByIdAsync(int id);
        Task<bool> CreateKhuyenMaiAsync(KhuyenMaiCreateDTO khuyenMai);
        Task<bool> UpdateKhuyenMaiAsync(int id, KhuyenMaiUpdateDTO khuyenMai);
        Task<bool> DeleteKhuyenMaiAsync(int id);
    }
}