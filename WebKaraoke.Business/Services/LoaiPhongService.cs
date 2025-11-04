using Microsoft.EntityFrameworkCore;
using WebKaraoke.Data;
using WebKaraoke.Data.Entities;
using WebKaraoke.DTO;
using WebKaraoke.Business.Interfaces;

namespace WebKaraoke.Business.Services
{
    public class LoaiPhongService : ILoaiPhongService
    {
        private readonly WebKaraokeDbContext _context;

        public LoaiPhongService(WebKaraokeDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LoaiPhongDTO>> GetAllLoaiPhongsAsync()
        {
            var loaiPhongs = await _context.LoaiPhongs
                .Include(lp => lp.Phongs)
                .OrderBy(lp => lp.TenLoai)
                .ToListAsync();

            return loaiPhongs.Select(lp => new LoaiPhongDTO
            {
                LoaiPhongID = lp.LoaiPhongID,
                TenLoai = lp.TenLoai,
                GiaPhong = lp.GiaPhong,
                SucChua = lp.SucChua,
                MoTa = lp.MoTa,
                SoLuongPhong = lp.Phongs.Count
            });
        }

        public async Task<LoaiPhongDTO?> GetLoaiPhongByIdAsync(int id)
        {
            var loaiPhong = await _context.LoaiPhongs
                .Include(lp => lp.Phongs)
                .FirstOrDefaultAsync(lp => lp.LoaiPhongID == id);

            if (loaiPhong == null) return null;

            return new LoaiPhongDTO
            {
                LoaiPhongID = loaiPhong.LoaiPhongID,
                TenLoai = loaiPhong.TenLoai,
                GiaPhong = loaiPhong.GiaPhong,
                SucChua = loaiPhong.SucChua,
                MoTa = loaiPhong.MoTa,
                SoLuongPhong = loaiPhong.Phongs.Count
            };
        }

        public async Task<bool> CreateLoaiPhongAsync(LoaiPhongCreateDTO loaiPhongDto)
        {
            var loaiPhong = new LoaiPhong
            {
                TenLoai = loaiPhongDto.TenLoai,
                GiaPhong = loaiPhongDto.GiaPhong,
                SucChua = loaiPhongDto.SucChua,
                MoTa = loaiPhongDto.MoTa
            };
            
            _context.LoaiPhongs.Add(loaiPhong);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateLoaiPhongAsync(int id, LoaiPhongUpdateDTO loaiPhongDto)
        {
            var loaiPhong = await _context.LoaiPhongs.FindAsync(id);
            if (loaiPhong == null) return false;

            loaiPhong.TenLoai = loaiPhongDto.TenLoai;
            loaiPhong.GiaPhong = loaiPhongDto.GiaPhong;
            loaiPhong.SucChua = loaiPhongDto.SucChua;
            loaiPhong.MoTa = loaiPhongDto.MoTa;

            _context.LoaiPhongs.Update(loaiPhong);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteLoaiPhongAsync(int id)
        {
            var loaiPhong = await _context.LoaiPhongs
                .Include(lp => lp.Phongs)
                .FirstOrDefaultAsync(lp => lp.LoaiPhongID == id);

            if (loaiPhong == null) return false;
            if (loaiPhong.Phongs.Any()) return false;

            _context.LoaiPhongs.Remove(loaiPhong);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<LoaiPhongDTO>> SearchLoaiPhongAsync(string tenLoai)
        {
            var loaiPhongs = await _context.LoaiPhongs
                .Include(lp => lp.Phongs)
                .Where(lp => lp.TenLoai.ToLower().Contains(tenLoai.ToLower()))
                .OrderBy(lp => lp.TenLoai)
                .ToListAsync();

            return loaiPhongs.Select(lp => new LoaiPhongDTO
            {
                LoaiPhongID = lp.LoaiPhongID,
                TenLoai = lp.TenLoai,
                GiaPhong = lp.GiaPhong,
                SucChua = lp.SucChua,
                MoTa = lp.MoTa,
                SoLuongPhong = lp.Phongs.Count
            });
        }
    }
}