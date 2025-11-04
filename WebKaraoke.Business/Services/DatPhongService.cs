using Microsoft.EntityFrameworkCore;
using WebKaraoke.Data;
using WebKaraoke.Data.Entities;
using WebKaraoke.DTO;
using WebKaraoke.Business.Interfaces;

namespace WebKaraoke.Business.Services
{
    public class DatPhongService : IDatPhongService
    {
        private readonly WebKaraokeDbContext _context;

        public DatPhongService(WebKaraokeDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DatPhongDTO>> GetDatPhongByKhachHangAsync(int khachHangId)
        {
            var datPhongs = await _context.DatPhongs
                .Include(dp => dp.Phong)
                    .ThenInclude(p => p.LoaiPhong)
                .Include(dp => dp.KhachHang)
                .Where(dp => dp.KhachHangID == khachHangId)
                .OrderByDescending(dp => dp.NgayDat)
                .ToListAsync();

            return datPhongs.Select(dp => new DatPhongDTO
            {
                DatPhongID = dp.DatPhongID,
                TenPhong = dp.Phong.TenPhong,
                TenKhachHang = dp.KhachHang.HoTen,
                LoaiPhong = dp.Phong.LoaiPhong.TenLoai,
                GiaGio = dp.Phong.GiaGio,
                NgayDat = dp.NgayDat,
                GioBatDau = dp.GioBatDau,
                GioKetThuc = dp.GioKetThuc,
                TrangThai = dp.TrangThai
            });
        }

        public async Task<DatPhongDTO?> GetDatPhongByIdAsync(int id)
        {
            var datPhong = await _context.DatPhongs
                .Include(dp => dp.Phong)
                    .ThenInclude(p => p.LoaiPhong)
                .Include(dp => dp.KhachHang)
                .FirstOrDefaultAsync(dp => dp.DatPhongID == id);

            if (datPhong == null) return null;

            return new DatPhongDTO
            {
                DatPhongID = datPhong.DatPhongID,
                TenPhong = datPhong.Phong.TenPhong,
                TenKhachHang = datPhong.KhachHang.HoTen,
                LoaiPhong = datPhong.Phong.LoaiPhong.TenLoai,
                GiaGio = datPhong.Phong.GiaGio,
                NgayDat = datPhong.NgayDat,
                GioBatDau = datPhong.GioBatDau,
                GioKetThuc = datPhong.GioKetThuc,
                TrangThai = datPhong.TrangThai
            };
        }

        public async Task<bool> HuyDatPhongAsync(int datPhongId, int khachHangId)
        {
            try
            {
                var datPhong = await _context.DatPhongs
                    .Include(dp => dp.Phong)
                    .FirstOrDefaultAsync(dp => dp.DatPhongID == datPhongId && dp.KhachHangID == khachHangId);

                if (datPhong == null) return false;

                datPhong.TrangThai = "DaHuy";
                datPhong.Phong.TrangThai = "Trong";

                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi hủy đặt phòng: {ex.Message}");
                return false;
            }
        }

        public async Task<IEnumerable<DatPhongDTO>> GetDatPhongChoXacNhanAsync()
        {
            var datPhongs = await _context.DatPhongs
                .Include(dp => dp.Phong)
                    .ThenInclude(p => p.LoaiPhong)
                .Include(dp => dp.KhachHang)
                .Where(dp => dp.TrangThai == "ChoXacNhan")
                .OrderBy(dp => dp.NgayDat)
                .ToListAsync();

            return datPhongs.Select(dp => new DatPhongDTO
            {
                DatPhongID = dp.DatPhongID,
                TenPhong = dp.Phong.TenPhong,
                TenKhachHang = dp.KhachHang.HoTen,
                LoaiPhong = dp.Phong.LoaiPhong.TenLoai,
                GiaGio = dp.Phong.GiaGio,
                NgayDat = dp.NgayDat,
                GioBatDau = dp.GioBatDau,
                GioKetThuc = dp.GioKetThuc,
                TrangThai = dp.TrangThai
            });
        }

        public async Task<bool> XacNhanDatPhongAsync(int datPhongId, int nhanVienId)
        {
            try
            {
                var datPhong = await _context.DatPhongs
                    .Include(dp => dp.Phong)
                    .FirstOrDefaultAsync(dp => dp.DatPhongID == datPhongId);

                if (datPhong == null) return false;

                datPhong.TrangThai = "DaXacNhan";
                datPhong.Phong.TrangThai = "DangSuDung";

                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi xác nhận đặt phòng: {ex.Message}");
                return false;
            }
        }
    }
}