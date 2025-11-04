using Microsoft.EntityFrameworkCore;
using WebKaraoke.Data;
using WebKaraoke.Data.Entities;
using WebKaraoke.DTO;
using WebKaraoke.Business.Interfaces;

namespace WebKaraoke.Business.Services
{
    public class ThongKeService : IThongKeService
    {
        private readonly WebKaraokeDbContext _context;

        public ThongKeService(WebKaraokeDbContext context)
        {
            _context = context;
        }

        public async Task<object> GetThongKeDoanhThuAsync(DateTime fromDate, DateTime toDate)
        {
            var doanhThuTheoThang = await _context.HoaDons
                .Where(hd => hd.NgayLap >= fromDate && hd.NgayLap <= toDate && hd.TrangThai == "DaThanhToan") // SỬA NgayTao -> NgayLap
                .GroupBy(hd => new { hd.NgayLap.Year, hd.NgayLap.Month }) // SỬA NgayTao -> NgayLap
                .Select(g => new
                {
                    Thang = $"{g.Key.Month}/{g.Key.Year}",
                    TongDoanhThu = g.Sum(hd => hd.TongTien),
                    SoHoaDon = g.Count()
                })
                .OrderBy(x => x.Thang)
                .ToListAsync();

            return doanhThuTheoThang;
        }

        public async Task<object> GetMonBanChayAsync(int top, int thang)
        {
            var monBanChay = await _context.ChiTietHoaDons
                .Include(ct => ct.MonAnNuocUong)
                .Include(ct => ct.HoaDon)
                .Where(ct => ct.HoaDon.NgayLap.Month == thang && ct.HoaDon.TrangThai == "DaThanhToan") // SỬA NgayTao -> NgayLap
                .GroupBy(ct => new { ct.MonID, ct.MonAnNuocUong.TenMon, ct.MonAnNuocUong.DanhMuc })
                .Select(g => new
                {
                    TenMon = g.Key.TenMon,
                    DanhMuc = g.Key.DanhMuc,
                    SoLuong = g.Sum(ct => ct.SoLuong),
                    TongTien = g.Sum(ct => ct.DonGia)
                })
                .OrderByDescending(x => x.SoLuong)
                .Take(top)
                .ToListAsync();

            return monBanChay;
        }

        public async Task<object> ThongKeDoanhThuTheoLoaiPhongAsync(DateTime fromDate, DateTime toDate)
        {
            var thongKe = await _context.HoaDons
                .Include(hd => hd.DatPhong)
                    .ThenInclude(dp => dp.Phong)
                        .ThenInclude(p => p.LoaiPhong)
                .Where(hd => hd.NgayLap >= fromDate && hd.NgayLap <= toDate && hd.TrangThai == "DaThanhToan") // SỬA NgayTao -> NgayLap
                .GroupBy(hd => hd.DatPhong.Phong.LoaiPhong.TenLoai)
                .Select(g => new
                {
                    LoaiPhong = g.Key,
                    TongDoanhThu = g.Sum(hd => hd.TongTien),
                    SoLuotDat = g.Count()
                })
                .ToListAsync();

            return thongKe;
        }
    }
}