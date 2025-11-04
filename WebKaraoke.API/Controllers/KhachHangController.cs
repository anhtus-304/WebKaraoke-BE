using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebKaraoke.Business.Interfaces;
using WebKaraoke.DTO;

namespace WebKaraoke.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class KhachHangController : ControllerBase
    {
        private readonly IKhachHangService _khachHangService;
        private readonly ILogger<KhachHangController> _logger;

        public KhachHangController(IKhachHangService khachHangService, ILogger<KhachHangController> logger)
        {
            _khachHangService = khachHangService;
            _logger = logger;
        }

        [HttpGet("me")]
        [ProducesResponseType(typeof(KhachHangDTO), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<ActionResult<KhachHangDTO>> GetCurrentKhachHang()
        {
            try
            {
                var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim))
                {
                    return Unauthorized(new ErrorResponse { Message = "Không tìm thấy thông tin người dùng" });
                }

                var khachHangId = int.Parse(userIdClaim);
                var khachHang = await _khachHangService.GetKhachHangByIdAsync(khachHangId);
                if (khachHang == null)
                {
                    return NotFound(new ErrorResponse { Message = "Không tìm thấy thông tin khách hàng" });
                }

                return Ok(khachHang);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting current customer info");
                return StatusCode(500, new ErrorResponse { Message = $"Lỗi server: {ex.Message}" });
            }
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "NhanVien,Admin")]
        [ProducesResponseType(typeof(KhachHangDTO), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<ActionResult<KhachHangDTO>> GetKhachHangById(int id)
        {
            try
            {
                var khachHang = await _khachHangService.GetKhachHangByIdAsync(id);
                if (khachHang == null)
                {
                    return NotFound(new ErrorResponse { Message = "Không tìm thấy khách hàng" });
                }

                return Ok(khachHang);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting customer by ID: {CustomerId}", id);
                return StatusCode(500, new ErrorResponse { Message = $"Lỗi server: {ex.Message}" });
            }
        }

        [HttpGet]
        [Authorize(Roles = "NhanVien,Admin")]
        [ProducesResponseType(typeof(IEnumerable<KhachHangDTO>), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<ActionResult<IEnumerable<KhachHangDTO>>> GetAllKhachHangs()
        {
            try
            {
                var khachHangs = await _khachHangService.GetAllKhachHangsAsync();
                return Ok(khachHangs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all customers");
                return StatusCode(500, new ErrorResponse { Message = $"Lỗi server: {ex.Message}" });
            }
        }

        [HttpPut("me")]
        [Authorize(Roles = "Khach")]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<ActionResult> UpdateCurrentKhachHang([FromBody] KhachHangUpdateDTO khachHang)
        {
            try
            {
                var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim))
                {
                    return Unauthorized(new ErrorResponse { Message = "Không tìm thấy thông tin người dùng" });
                }

                var khachHangId = int.Parse(userIdClaim);
                var result = await _khachHangService.UpdateKhachHangAsync(khachHangId, khachHang);

                if (!result)
                {
                    return BadRequest(new ErrorResponse { Message = "Không thể cập nhật thông tin khách hàng" });
                }

                return Ok(new { message = "Cập nhật thông tin thành công" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating customer info");
                return StatusCode(500, new ErrorResponse { Message = $"Lỗi server: {ex.Message}" });
            }
        }

        [HttpGet("me/diem-thanh-vien")]
        [Authorize(Roles = "Khach")]
        [ProducesResponseType(typeof(DiemThanhVienDTO), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<ActionResult<DiemThanhVienDTO>> GetDiemThanhVien()
        {
            try
            {
                var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim))
                {
                    return Unauthorized(new ErrorResponse { Message = "Không tìm thấy thông tin người dùng" });
                }

                var khachHangId = int.Parse(userIdClaim);
                var diemThanhVien = await _khachHangService.GetDiemThanhVienAsync(khachHangId);
                if (diemThanhVien == null)
                {
                    return NotFound(new ErrorResponse { Message = "Không tìm thấy thông tin điểm thành viên" });
                }

                return Ok(diemThanhVien);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting loyalty points for customer");
                return StatusCode(500, new ErrorResponse { Message = $"Lỗi server: {ex.Message}" });
            }
        }
    }
}