using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebKaraoke.Business.Interfaces;
using WebKaraoke.DTO;

namespace WebKaraoke.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DatPhongController : ControllerBase
    {
        private readonly IDatPhongService _datPhongService;
        private readonly ILogger<DatPhongController> _logger;

        public DatPhongController(IDatPhongService datPhongService, ILogger<DatPhongController> logger)
        {
            _datPhongService = datPhongService;
            _logger = logger;
        }

        [HttpGet("khach-hang")]
        [Authorize(Roles = "Khach")]
        [ProducesResponseType(typeof(IEnumerable<DatPhongDTO>), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<ActionResult<IEnumerable<DatPhongDTO>>> GetDatPhongByKhachHang()
        {
            try
            {
                var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim))
                {
                    return Unauthorized(new ErrorResponse { Message = "Không tìm thấy thông tin người dùng" });
                }

                var khachHangId = int.Parse(userIdClaim);
                var datPhongs = await _datPhongService.GetDatPhongByKhachHangAsync(khachHangId);
                return Ok(datPhongs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting bookings for customer");
                return StatusCode(500, new ErrorResponse { Message = $"Lỗi server: {ex.Message}" });
            }
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(DatPhongDTO), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<ActionResult<DatPhongDTO>> GetDatPhongById(int id)
        {
            try
            {
                var datPhong = await _datPhongService.GetDatPhongByIdAsync(id);
                if (datPhong == null)
                {
                    return NotFound(new ErrorResponse { Message = "Không tìm thấy đặt phòng" });
                }

                return Ok(datPhong);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting booking by ID: {BookingId}", id);
                return StatusCode(500, new ErrorResponse { Message = $"Lỗi server: {ex.Message}" });
            }
        }

        [HttpPut("{id:int}/huy")]
        [Authorize(Roles = "Khach")]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<ActionResult> HuyDatPhong(int id)
        {
            try
            {
                var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim))
                {
                    return Unauthorized(new ErrorResponse { Message = "Không tìm thấy thông tin người dùng" });
                }

                var khachHangId = int.Parse(userIdClaim);
                var result = await _datPhongService.HuyDatPhongAsync(id, khachHangId);

                if (!result)
                {
                    return BadRequest(new ErrorResponse { Message = "Không thể hủy đặt phòng" });
                }

                return Ok(new { message = "Hủy đặt phòng thành công" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cancelling booking {BookingId}", id);
                return StatusCode(500, new ErrorResponse { Message = $"Lỗi server: {ex.Message}" });
            }
        }

        [HttpGet("cho-xac-nhan")]
        [Authorize(Roles = "NhanVien,Admin")]
        [ProducesResponseType(typeof(IEnumerable<DatPhongDTO>), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<ActionResult<IEnumerable<DatPhongDTO>>> GetDatPhongChoXacNhan()
        {
            try
            {
                var datPhongs = await _datPhongService.GetDatPhongChoXacNhanAsync();
                return Ok(datPhongs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting pending confirmation bookings");
                return StatusCode(500, new ErrorResponse { Message = $"Lỗi server: {ex.Message}" });
            }
        }

        [HttpPut("{id:int}/xac-nhan")]
        [Authorize(Roles = "NhanVien,Admin")]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<ActionResult> XacNhanDatPhong(int id)
        {
            try
            {
                var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim))
                {
                    return Unauthorized(new ErrorResponse { Message = "Không tìm thấy thông tin người dùng" });
                }

                var nhanVienId = int.Parse(userIdClaim);
                var result = await _datPhongService.XacNhanDatPhongAsync(id, nhanVienId);

                if (!result)
                {
                    return BadRequest(new ErrorResponse { Message = "Không thể xác nhận đặt phòng" });
                }

                return Ok(new { message = "Xác nhận đặt phòng thành công" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error confirming booking {BookingId}", id);
                return StatusCode(500, new ErrorResponse { Message = $"Lỗi server: {ex.Message}" });
            }
        }
    }
}