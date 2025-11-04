using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebKaraoke.Business.Interfaces;
using WebKaraoke.DTO;

namespace WebKaraoke.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PhongController : ControllerBase
    {
        private readonly IPhongService _phongService;
        private readonly ILogger<PhongController> _logger;

        public PhongController(IPhongService phongService, ILogger<PhongController> logger)
        {
            _phongService = phongService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PhongDTO>), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<ActionResult<IEnumerable<PhongDTO>>> GetAllPhongs()
        {
            try
            {
                var phongs = await _phongService.GetAllPhongsAsync();
                return Ok(phongs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all rooms");
                return StatusCode(500, new ErrorResponse { Message = $"Lỗi server: {ex.Message}" });
            }
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(PhongDTO), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<ActionResult<PhongDTO>> GetPhongById(int id)
        {
            try
            {
                var phong = await _phongService.GetPhongByIdAsync(id);
                
                if (phong == null)
                {
                    return NotFound(new ErrorResponse { Message = "Không tìm thấy phòng" });
                }

                return Ok(phong);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting room by ID: {RoomId}", id);
                return StatusCode(500, new ErrorResponse { Message = $"Lỗi server: {ex.Message}" });
            }
        }

        [HttpGet("trong")]
        [ProducesResponseType(typeof(IEnumerable<PhongDTO>), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<ActionResult<IEnumerable<PhongDTO>>> GetAvailablePhongs(
            [FromQuery] DateTime ngay, 
            [FromQuery] string? loaiPhong = null)
        {
            try
            {
                var phongs = await _phongService.GetAvailablePhongsAsync(ngay, loaiPhong);
                return Ok(phongs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting available rooms for date: {Date}, type: {RoomType}", ngay, loaiPhong);
                return StatusCode(500, new ErrorResponse { Message = $"Lỗi server: {ex.Message}" });
            }
        }

        [HttpPost("dat-phong")]
        [Authorize(Roles = "Khach")]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<ActionResult> DatPhong([FromBody] DatPhongRequest request)
        {
            try
            {
                var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim))
                {
                    return Unauthorized(new ErrorResponse { Message = "Không tìm thấy thông tin người dùng" });
                }

                var khachHangId = int.Parse(userIdClaim);
                
                var result = await _phongService.DatPhongAsync(request, khachHangId);
                
                if (!result)
                {
                    return BadRequest(new ErrorResponse { Message = "Phòng không khả dụng trong khoảng thời gian này" });
                }

                return Ok(new { message = "Đặt phòng thành công, chờ xác nhận" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error booking room for customer: {CustomerId}", 
                    User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
                return StatusCode(500, new ErrorResponse { Message = $"Lỗi server: {ex.Message}" });
            }
        }
    }
}