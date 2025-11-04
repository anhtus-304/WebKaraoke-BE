using AutoMapper;
using WebKaraoke.Data;
using WebKaraoke.Data.Entities;
using WebKaraoke.Business.Interfaces;
using WebKaraoke.DTO;
using Microsoft.Extensions.Logging;

namespace WebKaraoke.Business.Services
{
    public class MonAnService : IMonAnService
    {
        private readonly IRepository<MonAnNuocUong> _monAnRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<MonAnService> _logger;

        public MonAnService(
            IRepository<MonAnNuocUong> monAnRepository,
            IMapper mapper,
            ILogger<MonAnService> logger)
        {
            _monAnRepository = monAnRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<MonAnNuocUongDTO>> GetAllMonAnAsync()
        {
            try
            {
                var monAns = await _monAnRepository.GetAllAsync();
                return _mapper.Map<IEnumerable<MonAnNuocUongDTO>>(monAns);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all menu items");
                throw;
            }
        }

        public async Task<IEnumerable<MonAnNuocUongDTO>> GetMonAnByDanhMucAsync(string danhMuc)
        {
            try
            {
                var monAns = (await _monAnRepository.GetAllAsync())
                    .Where(m => m.DanhMuc == danhMuc && m.DangKinhDoanh)
                    .OrderBy(m => m.TenMon);
                
                return _mapper.Map<IEnumerable<MonAnNuocUongDTO>>(monAns);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting menu items by category: {Category}", danhMuc);
                throw;
            }
        }

        public async Task<MonAnNuocUongDTO?> GetMonAnByIdAsync(int id)
        {
            try
            {
                var monAn = await _monAnRepository.GetByIdAsync(id);
                return _mapper.Map<MonAnNuocUongDTO?>(monAn);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting menu item by ID: {ItemId}", id);
                throw;
            }
        }

        public async Task<bool> AddMonAnAsync(MonAnNuocUongCreateDTO monAn)
        {
            try
            {
                var monAnEntity = _mapper.Map<MonAnNuocUong>(monAn);
                await _monAnRepository.AddAsync(monAnEntity);
                await _monAnRepository.SaveAsync();

                _logger.LogInformation("Menu item created successfully: {ItemName}", monAn.TenMon);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating menu item: {ItemName}", monAn.TenMon);
                throw;
            }
        }

        public async Task<bool> UpdateMonAnAsync(int id, MonAnNuocUongUpdateDTO monAn)
        {
            try
            {
                var existingMonAn = await _monAnRepository.GetByIdAsync(id);
                if (existingMonAn == null)
                {
                    _logger.LogWarning("Menu item {ItemId} not found for update", id);
                    return false;
                }

                _mapper.Map(monAn, existingMonAn);
                _monAnRepository.Update(existingMonAn);
                await _monAnRepository.SaveAsync();

                _logger.LogInformation("Menu item {ItemId} updated successfully", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating menu item: {ItemId}", id);
                throw;
            }
        }

        public async Task<bool> DeleteMonAnAsync(int id)
        {
            try
            {
                var monAn = await _monAnRepository.GetByIdAsync(id);
                if (monAn == null)
                {
                    _logger.LogWarning("Menu item {ItemId} not found for deletion", id);
                    return false;
                }

                // Soft delete by setting DangKinhDoanh to false
                monAn.DangKinhDoanh = false;
                _monAnRepository.Update(monAn);
                await _monAnRepository.SaveAsync();

                _logger.LogInformation("Menu item {ItemId} deleted successfully", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting menu item: {ItemId}", id);
                throw;
            }
        }
    }
}