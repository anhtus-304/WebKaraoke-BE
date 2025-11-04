using AutoMapper;
using WebKaraoke.Data;
using WebKaraoke.Data.Entities;
using WebKaraoke.Business.Interfaces;
using WebKaraoke.DTO;
using Microsoft.Extensions.Logging;
namespace WebKaraoke.Business.Services
{
    public class DatPhongService : IDatPhongService
    {
        private readonly IRepository<DatPhong> _datPhongRepository;
        private readonly IRepository<Phong> _phongRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<DatPhongService> _logger;

        public DatPhongService(
            IRepository<DatPhong> datPhongRepository,
            IRepository<Phong> phongRepository,
            IMapper mapper,
            ILogger<DatPhongService> logger)
        {
            _datPhongRepository = datPhongRepository;
            _phongRepository = phongRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<DatPhongDTO>> GetDatPhongByKhachHangAsync(int khachHangId)
        {
            try
            {
                var datPhongs = (await _datPhongRepository.GetAllAsync())
                    .Where(dp => dp.KhachHangID == khachHangId)
                    .OrderByDescending(dp => dp.ThoiGianDat);

                return _mapper.Map<IEnumerable<DatPhongDTO>>(datPhongs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting bookings for customer: {CustomerId}", khachHangId);
                throw;
            }
        }

        public async Task<DatPhongDTO?> GetDatPhongByIdAsync(int id)
        {
            try
            {
                var datPhong = await _datPhongRepository.GetByIdAsync(id);
                return _mapper.Map<DatPhongDTO?>(datPhong);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting booking by ID: {BookingId}", id);
                throw;
            }
        }

        public async Task<bool> HuyDatPhongAsync(int datPhongId, int khachHangId)
        {
            try
            {
                var datPhong = await _datPhongRepository.GetByIdAsync(datPhongId);
                if (datPhong == null || datPhong.KhachHangID != khachHangId)
                {
                    _logger.LogWarning("Booking {BookingId} not found or doesn't belong to customer {CustomerId}", 
                        datPhongId, khachHangId);
                    return false;
                }

                if (datPhong.TrangThai != "ChoXacNhan")
                {
                    _logger.LogWarning("Cannot cancel booking {BookingId} with status: {Status}", 
                        datPhongId, datPhong.TrangThai);
                    return false;
                }

                datPhong.TrangThai = "DaHuy";
                _datPhongRepository.Update(datPhong);

                // Update room status back to available
                var phong = await _phongRepository.GetByIdAsync(datPhong.PhongID);
                if (phong != null)
                {
                    phong.TrangThai = "Trong";
                    _phongRepository.Update(phong);
                }

                await _datPhongRepository.SaveAsync();

                _logger.LogInformation("Booking {BookingId} cancelled successfully by customer {CustomerId}", 
                    datPhongId, khachHangId);
                
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cancelling booking {BookingId} by customer {CustomerId}", 
                    datPhongId, khachHangId);
                throw;
            }
        }

        public async Task<IEnumerable<DatPhongDTO>> GetDatPhongChoXacNhanAsync()
        {
            try
            {
                var datPhongs = (await _datPhongRepository.GetAllAsync())
                    .Where(dp => dp.TrangThai == "ChoXacNhan")
                    .OrderBy(dp => dp.ThoiGianDat);

                return _mapper.Map<IEnumerable<DatPhongDTO>>(datPhongs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting pending confirmation bookings");
                throw;
            }
        }

        public async Task<bool> XacNhanDatPhongAsync(int datPhongId, int nhanVienId)
        {
            try
            {
                var datPhong = await _datPhongRepository.GetByIdAsync(datPhongId);
                if (datPhong == null || datPhong.TrangThai != "ChoXacNhan")
                {
                    _logger.LogWarning("Booking {BookingId} not found or not pending confirmation", datPhongId);
                    return false;
                }

                datPhong.TrangThai = "DaXacNhan";
                _datPhongRepository.Update(datPhong);
                await _datPhongRepository.SaveAsync();

                _logger.LogInformation("Booking {BookingId} confirmed by staff {StaffId}", datPhongId, nhanVienId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error confirming booking {BookingId} by staff {StaffId}", 
                    datPhongId, nhanVienId);
                throw;
            }
        }
    }
}