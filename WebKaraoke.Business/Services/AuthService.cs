using Microsoft.EntityFrameworkCore;
using AutoMapper;
using WebKaraoke.Data;
using WebKaraoke.Data.Entities;
using WebKaraoke.Business.Interfaces;
using WebKaraoke.DTO;
using WebKaraoke.Business.Helpers;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Logging;

namespace WebKaraoke.Business.Services
{
    public class AuthService : IAuthService
    {
        private readonly IRepository<TaiKhoan> _taiKhoanRepository;
        private readonly IRepository<KhachHang> _khachHangRepository;
        private readonly IRepository<NhanVien> _nhanVienRepository;
        private readonly IRepository<DiemThanhVien> _diemThanhVienRepository;
        private readonly JwtHelper _jwtHelper;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            IRepository<TaiKhoan> taiKhoanRepository,
            IRepository<KhachHang> khachHangRepository,
            IRepository<NhanVien> nhanVienRepository,
            IRepository<DiemThanhVien> diemThanhVienRepository,
            JwtHelper jwtHelper,
            IMapper mapper,
            ILogger<AuthService> logger)
        {
            _taiKhoanRepository = taiKhoanRepository;
            _khachHangRepository = khachHangRepository;
            _nhanVienRepository = nhanVienRepository;
            _diemThanhVienRepository = diemThanhVienRepository;
            _jwtHelper = jwtHelper;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<LoginResponse?> LoginAsync(LoginRequest request)
        {
            try
            {
                _logger.LogInformation("Attempting login for username: {Username}", request.Username);

                // Tìm tài khoản theo Username
                var taiKhoan = (await _taiKhoanRepository.GetAllAsync())
                    .FirstOrDefault(tk => tk.Username == request.Username);

                if (taiKhoan == null)
                {
                    _logger.LogWarning("User not found: {Username}", request.Username);
                    return null;
                }

                if (!VerifyPassword(request.Password, taiKhoan.MatKhauHash))
                {
                    _logger.LogWarning("Invalid password for user: {Username}", request.Username);
                    return null;
                }

                // Lấy thông tin họ tên theo role
                string hoTen = await GetHoTenByRoleAsync(taiKhoan);

                // Tạo token JWT
                var token = _jwtHelper.GenerateToken(
                    taiKhoan.Username,
                    taiKhoan.Role,
                    taiKhoan.TaiKhoanID,
                    hoTen);

                _logger.LogInformation("Login successful for user: {Username}", request.Username);

                return new LoginResponse
                {
                    Token = token,
                    Role = taiKhoan.Role,
                    HoTen = hoTen,
                    UserId = taiKhoan.TaiKhoanID,
                    Username = taiKhoan.Username
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for user: {Username}", request.Username);
                throw;
            }
        }

        public async Task<bool> RegisterAsync(RegisterRequest request)
        {
            try
            {
                _logger.LogInformation("Attempting registration for username: {Username}", request.Username);

                // Kiểm tra username đã tồn tại
                var existingTaiKhoan = (await _taiKhoanRepository.GetAllAsync())
                    .FirstOrDefault(tk => tk.Username == request.Username);

                if (existingTaiKhoan != null)
                {
                    _logger.LogWarning("Username already exists: {Username}", request.Username);
                    return false;
                }

                // Tạo khách hàng mới
                var khachHang = new KhachHang
                {
                    HoTen = request.Hoten ?? string.Empty,
                    Email = request.Email ?? string.Empty,
                    SoDienThoai = request.SoDienThoai ?? string.Empty
                };

                await _khachHangRepository.AddAsync(khachHang);
                await _khachHangRepository.SaveAsync();

                // Tạo tài khoản
                var taiKhoan = new TaiKhoan
                {
                    Username = request.Username,
                    MatKhauHash = HashPassword(request.Password),
                    Role = "Khach",
                    KhachHangID = khachHang.KhachHangID
                };

                await _taiKhoanRepository.AddAsync(taiKhoan);
                await _taiKhoanRepository.SaveAsync();

                // Tạo điểm thành viên
                var diemThanhVien = new DiemThanhVien
                {
                    KhachHangID = khachHang.KhachHangID,
                    TongDiem = 0,
                    DiemDaSuDung = 0,
                    NgayCapNhat = DateTime.UtcNow
                };

                await _diemThanhVienRepository.AddAsync(diemThanhVien);
                await _diemThanhVienRepository.SaveAsync();

                _logger.LogInformation("Registration successful for username: {Username}", request.Username);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during registration for username: {Username}", request.Username);
                throw;
            }
        }

        public async Task<object?> GetUserByIdAsync(int userId)
        {
            try
            {
                var taiKhoan = await _taiKhoanRepository.GetByIdAsync(userId);
                if (taiKhoan == null)
                {
                    _logger.LogWarning("User not found with ID: {UserId}", userId);
                    return null;
                }

                if (taiKhoan.Role == "Khach" && taiKhoan.KhachHangID.HasValue)
                {
                    var khachHang = await _khachHangRepository.GetByIdAsync(taiKhoan.KhachHangID.Value);
                    return new
                    {
                        taiKhoan.TaiKhoanID,
                        taiKhoan.Username,
                        taiKhoan.Role,
                        HoTen = khachHang?.HoTen,
                        Email = khachHang?.Email,
                        SoDienThoai = khachHang?.SoDienThoai
                    };
                }
                else if (taiKhoan.Role == "NhanVien" && taiKhoan.NhanVienID.HasValue)
                {
                    var nhanVien = await _nhanVienRepository.GetByIdAsync(taiKhoan.NhanVienID.Value);
                    return new
                    {
                        taiKhoan.TaiKhoanID,
                        taiKhoan.Username,
                        taiKhoan.Role,
                        HoTen = nhanVien?.HoTen,
                        SoDienThoai = nhanVien?.SoDienThoai,
                        ChucVu = nhanVien?.ChucVu
                    };
                }

                return new
                {
                    taiKhoan.TaiKhoanID,
                    taiKhoan.Username,
                    taiKhoan.Role,
                    HoTen = "Quản trị viên"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user by ID: {UserId}", userId);
                throw;
            }
        }

        public async Task<bool> ChangePasswordAsync(int userId, ChangePasswordRequest request)
        {
            try
            {
                var taiKhoan = await _taiKhoanRepository.GetByIdAsync(userId);
                if (taiKhoan == null)
                {
                    _logger.LogWarning("User not found for password change: {UserId}", userId);
                    return false;
                }

                if (!VerifyPassword(request.CurrentPassword, taiKhoan.MatKhauHash))
                {
                    _logger.LogWarning("Current password incorrect for user: {UserId}", userId);
                    return false;
                }

                taiKhoan.MatKhauHash = HashPassword(request.NewPassword);
                
                _taiKhoanRepository.Update(taiKhoan);
                await _taiKhoanRepository.SaveAsync();

                _logger.LogInformation("Password changed successfully for user: {UserId}", userId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error changing password for user: {UserId}", userId);
                throw;
            }
        }

        private async Task<string> GetHoTenByRoleAsync(TaiKhoan taiKhoan)
        {
            if (taiKhoan.Role == "Khach" && taiKhoan.KhachHangID.HasValue)
            {
                var khachHang = await _khachHangRepository.GetByIdAsync(taiKhoan.KhachHangID.Value);
                return khachHang?.HoTen ?? "Khách hàng";
            }
            else if (taiKhoan.Role == "NhanVien" && taiKhoan.NhanVienID.HasValue)
            {
                var nhanVien = await _nhanVienRepository.GetByIdAsync(taiKhoan.NhanVienID.Value);
                return nhanVien?.HoTen ?? "Nhân viên";
            }
            else if (taiKhoan.Role == "Admin")
            {
                return "Quản trị viên";
            }
            else
            {
                return "Người dùng";
            }
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            var hash = HashPassword(password);
            return hash == storedHash;
        }

        public async Task<bool> CheckUsernameExistsAsync(string username)
        {
            try
            {
                var existingUser = (await _taiKhoanRepository.GetAllAsync())
                    .FirstOrDefault(tk => tk.Username == username);
                return existingUser != null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking username existence: {Username}", username);
                throw;
            }
        }
    }
}