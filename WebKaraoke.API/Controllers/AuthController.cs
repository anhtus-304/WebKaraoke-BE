// Controllers/AuthController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebKaraoke.Business.Interfaces;
using WebKaraoke.DTO;

namespace WebKaraoke.API.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                var result = await _authService.RegisterAsync(request);
                return Success(result, "Đăng ký thành công");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Register failed");
                return Error(ex.Message, 500);
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var response = await _authService.LoginAsync(request);
                if (response == null) 
                    return UnauthorizedError("Sai tài khoản hoặc mật khẩu");
                
                return Success(response, "Đăng nhập thành công");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login failed");
                return Error(ex.Message, 500);
            }
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> Me()
        {
            try
            {
                var idClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(idClaim)) 
                    return UnauthorizedError("Không tìm thấy token");

                var userId = int.Parse(idClaim);
                var user = await _authService.GetUserByIdAsync(userId);
                
                return user == null ? NotFoundError("Không tìm thấy user") : Success(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Get profile failed");
                return Error(ex.Message, 500);
            }
        }
    }
}