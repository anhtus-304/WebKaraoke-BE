// WebKaraoke.Business/Interfaces/IThongKeService.cs
namespace WebKaraoke.Business.Interfaces
{
    public interface IThongKeService
    {
        Task<object> GetThongKeDoanhThuAsync(DateTime fromDate, DateTime toDate);
        Task<object> GetMonBanChayAsync(int top, int thang);
        Task<object> ThongKeDoanhThuTheoLoaiPhongAsync(DateTime fromDate, DateTime toDate);
    }
}