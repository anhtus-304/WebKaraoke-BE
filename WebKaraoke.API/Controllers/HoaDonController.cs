using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebKaraoke.Business.Interfaces;
using WebKaraoke.DTO;

namespace WebKaraoke.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class HoaDonController : ControllerBase
    {
        private readonly IHoaDonService _hoaDonService;
        private readonly ILogger<HoaDonController> _logger;

        public HoaDonController(IHoaDonService hoaDonService, ILogger<HoaDonController> logger)
        {
            _hoaDonService = hoaDonService;
            _logger = logger;
        }

        [HttpGet("dat-phong/{datPhongId:int}")]
        [ProducesResponseType(typeof(HoaDonDTO), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<ActionResult<HoaDonDTO>> GetHoaDonByDatPhong(int datPhongId)
        {
            try
            {
                var hoaDon = await _hoaDonService.GetHoaDonByDatPhongAsync(datPhongId);
                if (hoaDon == null)
                {
                    return NotFound(new ErrorResponse { Message = "Không tìm thấy hóa đơn" });
                }

                return Ok(hoaDon);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting invoice for booking: {BookingId}", datPhongId);
                return StatusCode(500, new ErrorResponse { Message = $"Lỗi server: {ex.Message}" });
            }
        }

        [HttpPost("dat-phong/{datPhongId:int}")]
        [Authorize(Roles = "NhanVien,Admin")]
        [ProducesResponseType(typeof(object), 201)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<ActionResult> TaoHoaDon(int datPhongId)
        {
            try
            {
                var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim))
                {
                    return Unauthorized(new ErrorResponse { Message = "Không tìm thấy thông tin người dùng" });
                }

                var nhanVienId = int.Parse(userIdClaim);
                var result = await _hoaDonService.TaoHoaDonAsync(datPhongId, nhanVienId);

                if (!result)
                {
                    return BadRequest(new ErrorResponse { Message = "Không thể tạo hóa đơn" });
                }

                return CreatedAtAction(nameof(GetHoaDonByDatPhong), new { datPhongId }, new { message = "Tạo hóa đơn thành công" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating invoice for booking: {BookingId}", datPhongId);
                return StatusCode(500, new ErrorResponse { Message = $"Lỗi server: {ex.Message}" });
            }
        }

        [HttpPost("{hoaDonId:int}/them-mon")]
        [Authorize(Roles = "NhanVien,Admin")]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<ActionResult> ThemMonVaoHoaDon(int hoaDonId, [FromBody] ThemMonVaoHoaDonRequest request)
        {
            try
            {
                var result = await _hoaDonService.ThemMonVaoHoaDonAsync(hoaDonId, request.MonID, request.SoLuong);
                if (!result)
                {
                    return BadRequest(new ErrorResponse { Message = "Không thể thêm món vào hóa đơn" });
                }

                return Ok(new { message = "Thêm món vào hóa đơn thành công" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding item to invoice: {InvoiceId}", hoaDonId);
                return StatusCode(500, new ErrorResponse { Message = $"Lỗi server: {ex.Message}" });
            }
        }

        [HttpPost("{hoaDonId:int}/ap-dung-khuyen-mai")]
        [Authorize(Roles = "NhanVien,Admin")]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<ActionResult> ApDungKhuyenMai(int hoaDonId, [FromBody] ApDungKhuyenMaiRequest request)
        {
            try
            {
                var result = await _hoaDonService.ApDungKhuyenMaiAsync(hoaDonId, request.MaKhuyenMai);
                if (!result)
                {
                    return BadRequest(new ErrorResponse { Message = "Không thể áp dụng khuyến mãi" });
                }

                return Ok(new { message = "Áp dụng khuyến mãi thành công" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error applying promotion to invoice: {InvoiceId}", hoaDonId);
                return StatusCode(500, new ErrorResponse { Message = $"Lỗi server: {ex.Message}" });
            }
        }

        [HttpGet("{hoaDonId:int}/tong-tien")]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<ActionResult> TinhTongTienHoaDon(int hoaDonId)
        {
            try
            {
                var tongTien = await _hoaDonService.TinhTongTienHoaDonAsync(hoaDonId);
                return Ok(new { tongTien });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating total for invoice: {InvoiceId}", hoaDonId);
                return StatusCode(500, new ErrorResponse { Message = $"Lỗi server: {ex.Message}" });
            }
        }
    }
}