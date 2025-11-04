using WebKaraoke.DTO;
using Microsoft.Extensions.Logging;
namespace WebKaraoke.Business.Interfaces
{
    public interface IThongKeService
    {
        Task<ThongKeDoanhThuDTO> GetThongKeDoanhThuAsync(DateTime tuNgay, DateTime denNgay);
        Task<IEnumerable<MonBanChayDTO>> GetMonBanChayAsync(int thang, int nam);
    }
}