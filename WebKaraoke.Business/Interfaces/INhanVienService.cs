using WebKaraoke.DTO;
using Microsoft.Extensions.Logging;
namespace WebKaraoke.Business.Interfaces
{
    public interface INhanVienService
    {
        Task<IEnumerable<NhanVienDTO>> GetAllNhanViensAsync();
        Task<NhanVienDTO?> GetNhanVienByIdAsync(int id);
        Task<bool> CreateNhanVienAsync(NhanVienCreateDTO nhanVien);
        Task<bool> UpdateNhanVienAsync(int id, NhanVienUpdateDTO nhanVien);
        Task<bool> DeleteNhanVienAsync(int id);
    }
}