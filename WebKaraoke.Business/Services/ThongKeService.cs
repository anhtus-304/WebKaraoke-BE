using Microsoft.EntityFrameworkCore;
using WebKaraoke.Data;
using WebKaraoke.Data.Entities;
using WebKaraoke.Business.Interfaces;
using WebKaraoke.DTO;
using Microsoft.Extensions.Logging;

namespace WebKaraoke.Business.Services
{
    public class ThongKeService : IThongKeService
    {
        private readonly WebKaraokeDbContext _context;
        private readonly ILogger<ThongKeService> _logger;

        public ThongKeService(WebKaraokeDbContext context, ILogger<ThongKeService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ThongKeDoanhThuDTO> GetThongKeDoanhThuAsync(DateTime tuNgay, DateTime denNgay)
        {
            try
            {
                var hoaDons = await _context.HoaDons
                    .Include(h => h.DatPhong)
                        .ThenInclude(dp => dp.Phong)
                            .ThenInclude(p => p.LoaiPhong) // THÊM: Include LoaiPhong
                    .Include(h => h.ChiTietHoaDons)
                        .ThenInclude(ct => ct.MonAnNuocUong)
                    .Where(h => h.NgayLap >= tuNgay && h.NgayLap <= denNgay && h.TrangThai == "DaThanhToan")
                    .ToListAsync();

                var thongKe = new ThongKeDoanhThuDTO
                {
                    TuNgay = tuNgay,
                    DenNgay = denNgay,
                    TongDoanhThu = hoaDons.Sum(h => h.TongTien),
                    TongHoaDon = hoaDons.Count,
                    TongKhachHang = hoaDons.Select(h => h.DatPhong.KhachHangID).Distinct().Count()
                };

                // Doanh thu theo ngày
                thongKe.DoanhThuTheoNgay = hoaDons
                    .GroupBy(h => h.NgayLap.Date)
                    .Select(g => new DoanhThuTheoNgayDTO
                    {
                        Ngay = g.Key,
                        DoanhThu = g.Sum(h => h.TongTien),
                        SoHoaDon = g.Count()
                    })
                    .OrderBy(d => d.Ngay)
                    .ToList();

                // Doanh thu theo phòng - SỬA: Lấy TenLoai từ LoaiPhong entity
                thongKe.DoanhThuTheoPhong = hoaDons
                    .GroupBy(h => new { 
                        h.DatPhong.Phong.TenPhong, 
                        LoaiPhong = h.DatPhong.Phong.LoaiPhong.TenLoai // SỬA: Lấy TenLoai
                    })
                    .Select(g => new DoanhThuTheoPhongDTO
                    {
                        TenPhong = g.Key.TenPhong,
                        LoaiPhong = g.Key.LoaiPhong, // Giờ là string từ TenLoai
                        DoanhThu = g.Sum(h => h.TongTien),
                        SoLanSuDung = g.Count()
                    })
                    .OrderByDescending(d => d.DoanhThu)
                    .ToList();

                // Món bán chạy
                thongKe.MonBanChay = hoaDons
                    .SelectMany(h => h.ChiTietHoaDons)
                    .GroupBy(ct => new { ct.MonID, ct.MonAnNuocUong.TenMon, ct.MonAnNuocUong.DanhMuc })
                    .Select(g => new MonBanChayDTO
                    {
                        MonID = g.Key.MonID,
                        TenMon = g.Key.TenMon,
                        DanhMuc = g.Key.DanhMuc,
                        SoLuongBan = g.Sum(ct => ct.SoLuong),
                        DoanhThu = g.Sum(ct => ct.SoLuong * ct.DonGia)
                    })
                    .OrderByDescending(m => m.SoLuongBan)
                    .Take(10)
                    .ToList();

                return thongKe;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting revenue statistics from {FromDate} to {ToDate}", tuNgay, denNgay);
                throw;
            }
        }

        public async Task<IEnumerable<MonBanChayDTO>> GetMonBanChayAsync(int thang, int nam)
        {
            try
            {
                var startDate = new DateTime(nam, thang, 1);
                var endDate = startDate.AddMonths(1).AddDays(-1);

                var chiTietHoaDons = await _context.ChiTietHoaDons
                    .Include(ct => ct.HoaDon)
                    .Include(ct => ct.MonAnNuocUong)
                    .Where(ct => ct.HoaDon.NgayLap >= startDate && ct.HoaDon.NgayLap <= endDate && ct.HoaDon.TrangThai == "DaThanhToan")
                    .ToListAsync();

                var monBanChay = chiTietHoaDons
                    .GroupBy(ct => new { ct.MonID, ct.MonAnNuocUong.TenMon, ct.MonAnNuocUong.DanhMuc })
                    .Select(g => new MonBanChayDTO
                    {
                        MonID = g.Key.MonID,
                        TenMon = g.Key.TenMon,
                        DanhMuc = g.Key.DanhMuc,
                        SoLuongBan = g.Sum(ct => ct.SoLuong),
                        DoanhThu = g.Sum(ct => ct.SoLuong * ct.DonGia)
                    })
                    .OrderByDescending(m => m.SoLuongBan)
                    .Take(20)
                    .ToList();

                return monBanChay;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting best-selling items for {Month}/{Year}", thang, nam);
                throw;
            }
        }
    }
}