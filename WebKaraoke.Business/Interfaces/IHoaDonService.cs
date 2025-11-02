using WebKaraoke.DTO;
using Microsoft.Extensions.Logging;
namespace WebKaraoke.Business.Interfaces
{
    public interface IHoaDonService
    {
        Task<HoaDonDTO?> GetHoaDonByDatPhongAsync(int datPhongId);
        Task<bool> TaoHoaDonAsync(int datPhongId, int nhanVienId);
        Task<bool> ThemMonVaoHoaDonAsync(int hoaDonId, int monId, int soLuong);
        Task<bool> ApDungKhuyenMaiAsync(int hoaDonId, string maKhuyenMai);
        Task<decimal> TinhTongTienHoaDonAsync(int hoaDonId);
    }
}