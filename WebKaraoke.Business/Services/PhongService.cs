using Microsoft.EntityFrameworkCore;
using AutoMapper;
using WebKaraoke.Data;
using WebKaraoke.Data.Entities;
using WebKaraoke.Business.Interfaces;
using WebKaraoke.DTO;
using Microsoft.Extensions.Logging;

namespace WebKaraoke.Business.Services
{
    public class PhongService : IPhongService
    {
        private readonly IRepository<Phong> _phongRepository;
        private readonly IRepository<DatPhong> _datPhongRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<PhongService> _logger;

        public PhongService(
            IRepository<Phong> phongRepository,
            IRepository<DatPhong> datPhongRepository,
            IMapper mapper,
            ILogger<PhongService> logger)
        {
            _phongRepository = phongRepository;
            _datPhongRepository = datPhongRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<PhongDTO>> GetAllPhongsAsync()
        {
            try
            {
                var phongs = await _phongRepository.GetAllAsync();
                return _mapper.Map<IEnumerable<PhongDTO>>(phongs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all rooms");
                throw;
            }
        }

        public async Task<PhongDTO?> GetPhongByIdAsync(int id)
        {
            try
            {
                var phong = await _phongRepository.GetByIdAsync(id);
                return _mapper.Map<PhongDTO?>(phong);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting room by ID: {RoomId}", id);
                throw;
            }
        }

        public async Task<IEnumerable<PhongDTO>> GetAvailablePhongsAsync(DateTime ngay, string? loaiPhong = null)
        {
            try
            {
                var allPhongs = await _phongRepository.GetAllAsync();
                
                // Filter by room type if specified
                if (!string.IsNullOrEmpty(loaiPhong))
                {
                    // SỬA: So sánh property TenLoai của LoaiPhong entity
                    allPhongs = allPhongs.Where(p => p.LoaiPhong.TenLoai == loaiPhong);
                }

                // Get booked room IDs for the specified date
                var bookedRoomIds = await GetBookedRoomIdsForDateAsync(ngay);
                
                // Get available rooms (not booked and status is "Trong")
                var availableRooms = allPhongs.Where(p => 
                    !bookedRoomIds.Contains(p.PhongID) && 
                    p.TrangThai == "Trong");

                return _mapper.Map<IEnumerable<PhongDTO>>(availableRooms);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting available rooms for date: {Date}, type: {RoomType}", ngay, loaiPhong);
                throw;
            }
        }

        public async Task<bool> DatPhongAsync(DatPhongRequest request, int khachHangId)
        {
            try
            {
                // Check if room is available
                var isAvailable = await IsRoomAvailableAsync(request.PhongID, request.GioBatDau, request.GioKetThuc);
                
                if (!isAvailable)
                {
                    _logger.LogWarning("Room {RoomId} is not available for booking", request.PhongID);
                    return false;
                }

                var datPhong = new DatPhong
                {
                    KhachHangID = khachHangId,
                    PhongID = request.PhongID,
                    ThoiGianDat = DateTime.UtcNow,
                    GioBatDau = request.GioBatDau,
                    GioKetThuc = request.GioKetThuc,
                    SoLuongNguoi = request.SoLuongNguoi,
                    TrangThai = "ChoXacNhan"
                };

                await _datPhongRepository.AddAsync(datPhong);
                await _datPhongRepository.SaveAsync();

                // Update room status
                var phong = await _phongRepository.GetByIdAsync(request.PhongID);
                if (phong != null)
                {
                    phong.TrangThai = "DaDat";
                    _phongRepository.Update(phong);
                    await _phongRepository.SaveAsync();
                }

                _logger.LogInformation("Room {RoomId} booked successfully by customer {CustomerId}", 
                    request.PhongID, khachHangId);
                
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error booking room {RoomId} for customer {CustomerId}", 
                    request.PhongID, khachHangId);
                throw;
            }
        }

        public async Task<bool> UpdateTrangThaiPhongAsync(int phongId, string trangThai)
        {
            try
            {
                var phong = await _phongRepository.GetByIdAsync(phongId);
                if (phong == null)
                {
                    _logger.LogWarning("Room {RoomId} not found for status update", phongId);
                    return false;
                }

                phong.TrangThai = trangThai;
                _phongRepository.Update(phong);
                await _phongRepository.SaveAsync();

                _logger.LogInformation("Room {RoomId} status updated to {Status}", phongId, trangThai);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating room {RoomId} status to {Status}", phongId, trangThai);
                throw;
            }
        }

        private async Task<List<int>> GetBookedRoomIdsForDateAsync(DateTime ngay)
        {
            var datPhongs = await _datPhongRepository.GetAllAsync();
            return datPhongs
                .Where(dp => dp.GioBatDau.Date == ngay.Date && 
                            dp.TrangThai != "DaHuy")
                .Select(dp => dp.PhongID)
                .ToList();
        }

        private async Task<bool> IsRoomAvailableAsync(int phongId, DateTime gioBatDau, DateTime gioKetThuc)
        {
            var datPhongs = await _datPhongRepository.GetAllAsync();
            return !datPhongs.Any(dp => 
                dp.PhongID == phongId && 
                dp.TrangThai != "DaHuy" &&
                gioBatDau < dp.GioKetThuc && 
                gioKetThuc > dp.GioBatDau);
        }
    }
}