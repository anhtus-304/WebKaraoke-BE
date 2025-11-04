using AutoMapper;
using WebKaraoke.Data;
using WebKaraoke.Data.Entities;
using WebKaraoke.Business.Interfaces;
using WebKaraoke.DTO;
using Microsoft.Extensions.Logging;

namespace WebKaraoke.Business.Services
{
    public class KhachHangService : IKhachHangService
    {
        private readonly IRepository<KhachHang> _khachHangRepository;
        private readonly IRepository<DiemThanhVien> _diemThanhVienRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<KhachHangService> _logger;

        public KhachHangService(
            IRepository<KhachHang> khachHangRepository,
            IRepository<DiemThanhVien> diemThanhVienRepository,
            IMapper mapper,
            ILogger<KhachHangService> logger)
        {
            _khachHangRepository = khachHangRepository;
            _diemThanhVienRepository = diemThanhVienRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<KhachHangDTO?> GetKhachHangByIdAsync(int id)
        {
            try
            {
                var khachHang = await _khachHangRepository.GetByIdAsync(id);
                return _mapper.Map<KhachHangDTO?>(khachHang);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting customer by ID: {CustomerId}", id);
                throw;
            }
        }

        public async Task<bool> UpdateKhachHangAsync(int id, KhachHangUpdateDTO khachHang)
        {
            try
            {
                var existingKhachHang = await _khachHangRepository.GetByIdAsync(id);
                if (existingKhachHang == null)
                {
                    _logger.LogWarning("Customer {CustomerId} not found for update", id);
                    return false;
                }

                _mapper.Map(khachHang, existingKhachHang);
                _khachHangRepository.Update(existingKhachHang);
                await _khachHangRepository.SaveAsync();

                _logger.LogInformation("Customer {CustomerId} updated successfully", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating customer: {CustomerId}", id);
                throw;
            }
        }

        public async Task<IEnumerable<KhachHangDTO>> GetAllKhachHangsAsync()
        {
            try
            {
                var khachHangs = await _khachHangRepository.GetAllAsync();
                return _mapper.Map<IEnumerable<KhachHangDTO>>(khachHangs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all customers");
                throw;
            }
        }

        public async Task<DiemThanhVienDTO?> GetDiemThanhVienAsync(int khachHangId)
        {
            try
            {
                var diemThanhVien = (await _diemThanhVienRepository.GetAllAsync())
                    .FirstOrDefault(d => d.KhachHangID == khachHangId);
                
                return _mapper.Map<DiemThanhVienDTO?>(diemThanhVien);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting loyalty points for customer: {CustomerId}", khachHangId);
                throw;
            }
        }
    }
}