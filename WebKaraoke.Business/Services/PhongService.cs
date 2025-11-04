using Microsoft.EntityFrameworkCore;
using WebKaraoke.Data;
using WebKaraoke.Data.Entities;
using WebKaraoke.DTO;
using WebKaraoke.Business.Interfaces;

namespace WebKaraoke.Business.Services
{
    public class PhongService : IPhongService
    {
        private readonly WebKaraokeDbContext _context;

        public PhongService(WebKaraokeDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PhongDTO>> GetAllPhongsAsync()
        {
            var phongs = await _context.Phongs
                .Include(p => p.LoaiPhong)
                .OrderBy(p => p.TenPhong)
                .ToListAsync();

            return phongs.Select(p => new PhongDTO
            {
                PhongID = p.PhongID,
                TenPhong = p.TenPhong,
                LoaiPhong = p.LoaiPhong.TenLoai,
                GiaGio = p.GiaGio,
                TrangThai = p.TrangThai
            });
        }

        public async Task<PhongDTO?> GetPhongByIdAsync(int id)
        {
            var phong = await _context.Phongs
                .Include(p => p.LoaiPhong)
                .FirstOrDefaultAsync(p => p.PhongID == id);

            if (phong == null) return null;

            return new PhongDTO
            {
                PhongID = phong.PhongID,
                TenPhong = phong.TenPhong,
                LoaiPhong = phong.LoaiPhong.TenLoai,
                GiaGio = phong.GiaGio,
                TrangThai = phong.TrangThai
            };
        }

        public async Task<IEnumerable<PhongDTO>> GetAvailablePhongsAsync(DateTime ngay, string? loaiPhong = null)
        {
            var query = _context.Phongs
                .Include(p => p.LoaiPhong)
                .Where(p => p.TrangThai == "Trong");

            if (!string.IsNullOrEmpty(loaiPhong))
            {
                query = query.Where(p => p.LoaiPhong.TenLoai == loaiPhong);
            }

            var phongs = await query.OrderBy(p => p.TenPhong).ToListAsync();

            return phongs.Select(p => new PhongDTO
            {
                PhongID = p.PhongID,
                TenPhong = p.TenPhong,
                LoaiPhong = p.LoaiPhong.TenLoai,
                GiaGio = p.GiaGio,
                TrangThai = p.TrangThai
            });
        }

        public async Task<bool> DatPhongAsync(DatPhongRequest request, int khachHangId)
        {
            try
            {
                var phong = await _context.Phongs.FindAsync(request.PhongID);
                if (phong == null || phong.TrangThai != "Trong")
                    return false;

                // Tạo đặt phòng với property đúng (ThoiGianDat)
                var datPhong = new DatPhong
                {
                    PhongID = request.PhongID,
                    KhachHangID = khachHangId,
                    NgayDat = DateTime.Now, // Sử dụng ThoiGianDat thay vì NgayDat
                    GioBatDau = request.GioBatDau,
                    GioKetThuc = request.GioKetThuc,
                    TrangThai = "ChoXacNhan"
                };

                _context.DatPhongs.Add(datPhong);
                phong.TrangThai = "DaDat";
                
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                // Log lỗi nếu cần
                Console.WriteLine($"Lỗi khi đặt phòng: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateTrangThaiPhongAsync(int phongId, string trangThai)
        {
            try
            {
                var phong = await _context.Phongs.FindAsync(phongId);
                if (phong == null) return false;

                phong.TrangThai = trangThai;
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                // Log lỗi nếu cần
                Console.WriteLine($"Lỗi khi cập nhật trạng thái phòng: {ex.Message}");
                return false;
            }
        }
    }
}