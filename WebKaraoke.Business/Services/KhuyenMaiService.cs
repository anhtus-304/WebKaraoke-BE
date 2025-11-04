using AutoMapper;
using WebKaraoke.Data;
using WebKaraoke.Data.Entities;
using WebKaraoke.Business.Interfaces;
using WebKaraoke.DTO;
using Microsoft.Extensions.Logging;
namespace WebKaraoke.Business.Services
{
    public class KhuyenMaiService : IKhuyenMaiService
    {
        private readonly IRepository<KhuyenMai> _khuyenMaiRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<KhuyenMaiService> _logger;

        public KhuyenMaiService(
            IRepository<KhuyenMai> khuyenMaiRepository,
            IMapper mapper,
            ILogger<KhuyenMaiService> logger)
        {
            _khuyenMaiRepository = khuyenMaiRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<KhuyenMaiDTO>> GetAllKhuyenMaiAsync()
        {
            try
            {
                var khuyenMais = await _khuyenMaiRepository.GetAllAsync();
                return _mapper.Map<IEnumerable<KhuyenMaiDTO>>(khuyenMais);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all promotions");
                throw;
            }
        }

        public async Task<KhuyenMaiDTO?> GetKhuyenMaiByIdAsync(int id)
        {
            try
            {
                var khuyenMai = await _khuyenMaiRepository.GetByIdAsync(id);
                return _mapper.Map<KhuyenMaiDTO?>(khuyenMai);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting promotion by ID: {PromotionId}", id);
                throw;
            }
        }

        public async Task<bool> CreateKhuyenMaiAsync(KhuyenMaiCreateDTO khuyenMai)
        {
            try
            {
                // Check if promotion code already exists
                var existingKhuyenMai = (await _khuyenMaiRepository.GetAllAsync())
                    .FirstOrDefault(km => km.MaKM == khuyenMai.MaKM);
                
                if (existingKhuyenMai != null)
                {
                    _logger.LogWarning("Promotion code {PromotionCode} already exists", khuyenMai.MaKM);
                    return false;
                }

                var khuyenMaiEntity = _mapper.Map<KhuyenMai>(khuyenMai);
                await _khuyenMaiRepository.AddAsync(khuyenMaiEntity);
                await _khuyenMaiRepository.SaveAsync();

                _logger.LogInformation("Promotion created successfully: {PromotionCode}", khuyenMai.MaKM);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating promotion: {PromotionCode}", khuyenMai.MaKM);
                throw;
            }
        }

        public async Task<bool> UpdateKhuyenMaiAsync(int id, KhuyenMaiUpdateDTO khuyenMai)
        {
            try
            {
                var existingKhuyenMai = await _khuyenMaiRepository.GetByIdAsync(id);
                if (existingKhuyenMai == null)
                {
                    _logger.LogWarning("Promotion {PromotionId} not found for update", id);
                    return false;
                }

                // Check if promotion code is being changed and already exists
                if (existingKhuyenMai.MaKM != khuyenMai.MaKM)
                {
                    var duplicateKhuyenMai = (await _khuyenMaiRepository.GetAllAsync())
                        .FirstOrDefault(km => km.MaKM == khuyenMai.MaKM && km.KM_ID != id);
                    
                    if (duplicateKhuyenMai != null)
                    {
                        _logger.LogWarning("Promotion code {PromotionCode} already exists", khuyenMai.MaKM);
                        return false;
                    }
                }

                _mapper.Map(khuyenMai, existingKhuyenMai);
                _khuyenMaiRepository.Update(existingKhuyenMai);
                await _khuyenMaiRepository.SaveAsync();

                _logger.LogInformation("Promotion {PromotionId} updated successfully", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating promotion: {PromotionId}", id);
                throw;
            }
        }

        public async Task<bool> DeleteKhuyenMaiAsync(int id)
        {
            try
            {
                var khuyenMai = await _khuyenMaiRepository.GetByIdAsync(id);
                if (khuyenMai == null)
                {
                    _logger.LogWarning("Promotion {PromotionId} not found for deletion", id);
                    return false;
                }

                _khuyenMaiRepository.Delete(khuyenMai);
                await _khuyenMaiRepository.SaveAsync();

                _logger.LogInformation("Promotion {PromotionId} deleted successfully", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting promotion: {PromotionId}", id);
                throw;
            }
        }
    }
}