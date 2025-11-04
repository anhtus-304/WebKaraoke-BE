using AutoMapper;
using WebKaraoke.Data;
using WebKaraoke.Data.Entities;
using WebKaraoke.Business.Interfaces;
using WebKaraoke.DTO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Logging;
namespace WebKaraoke.Business.Services
{
    public class NhanVienService : INhanVienService
    {
        private readonly IRepository<NhanVien> _nhanVienRepository;
        private readonly IRepository<TaiKhoan> _taiKhoanRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<NhanVienService> _logger;

        public NhanVienService(
            IRepository<NhanVien> nhanVienRepository,
            IRepository<TaiKhoan> taiKhoanRepository,
            IMapper mapper,
            ILogger<NhanVienService> logger)
        {
            _nhanVienRepository = nhanVienRepository;
            _taiKhoanRepository = taiKhoanRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<NhanVienDTO>> GetAllNhanViensAsync()
        {
            try
            {
                var nhanViens = await _nhanVienRepository.GetAllAsync();
                return _mapper.Map<IEnumerable<NhanVienDTO>>(nhanViens);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all employees");
                throw;
            }
        }

        public async Task<NhanVienDTO?> GetNhanVienByIdAsync(int id)
        {
            try
            {
                var nhanVien = await _nhanVienRepository.GetByIdAsync(id);
                return _mapper.Map<NhanVienDTO?>(nhanVien);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting employee by ID: {EmployeeId}", id);
                throw;
            }
        }

        public async Task<bool> CreateNhanVienAsync(NhanVienCreateDTO nhanVien)
        {
            try
            {
                // Check if username already exists
                var existingTaiKhoan = (await _taiKhoanRepository.GetAllAsync())
                    .FirstOrDefault(tk => tk.Username == nhanVien.Username);
                
                if (existingTaiKhoan != null)
                {
                    _logger.LogWarning("Username {Username} already exists", nhanVien.Username);
                    return false;
                }

                // Create NhanVien
                var nhanVienEntity = _mapper.Map<NhanVien>(nhanVien);
                await _nhanVienRepository.AddAsync(nhanVienEntity);
                await _nhanVienRepository.SaveAsync();

                // Create TaiKhoan
                var taiKhoan = new TaiKhoan
                {
                    Username = nhanVien.Username,
                    MatKhauHash = HashPassword(nhanVien.Password),
                    Role = "NhanVien",
                    NhanVienID = nhanVienEntity.NhanVienID
                };

                await _taiKhoanRepository.AddAsync(taiKhoan);
                await _taiKhoanRepository.SaveAsync();

                _logger.LogInformation("Employee created successfully: {Username}", nhanVien.Username);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating employee: {Username}", nhanVien.Username);
                throw;
            }
        }

        public async Task<bool> UpdateNhanVienAsync(int id, NhanVienUpdateDTO nhanVien)
        {
            try
            {
                var existingNhanVien = await _nhanVienRepository.GetByIdAsync(id);
                if (existingNhanVien == null)
                {
                    _logger.LogWarning("Employee {EmployeeId} not found for update", id);
                    return false;
                }

                _mapper.Map(nhanVien, existingNhanVien);
                _nhanVienRepository.Update(existingNhanVien);
                await _nhanVienRepository.SaveAsync();

                _logger.LogInformation("Employee {EmployeeId} updated successfully", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating employee: {EmployeeId}", id);
                throw;
            }
        }

        public async Task<bool> DeleteNhanVienAsync(int id)
        {
            try
            {
                var nhanVien = await _nhanVienRepository.GetByIdAsync(id);
                if (nhanVien == null)
                {
                    _logger.LogWarning("Employee {EmployeeId} not found for deletion", id);
                    return false;
                }

                // Delete associated TaiKhoan
                var taiKhoan = (await _taiKhoanRepository.GetAllAsync())
                    .FirstOrDefault(tk => tk.NhanVienID == id);
                
                if (taiKhoan != null)
                {
                    _taiKhoanRepository.Delete(taiKhoan);
                }

                _nhanVienRepository.Delete(nhanVien);
                await _nhanVienRepository.SaveAsync();

                _logger.LogInformation("Employee {EmployeeId} deleted successfully", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting employee: {EmployeeId}", id);
                throw;
            }
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}