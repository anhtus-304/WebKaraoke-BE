using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebKaraoke.Business.Interfaces;
using WebKaraoke.DTO;
using WebKaraoke.API.Helpers;

namespace WebKaraoke.API.Controllers
{
    [ApiController]
    [Route("api/v1/khuyenmai")]
    [Authorize]
    public class KhuyenMaiController : ControllerBase
    {
        private readonly IKhuyenMaiService _khuyenMaiService;
        private readonly ILogger<KhuyenMaiController> _logger;

        public KhuyenMaiController(
            IKhuyenMaiService khuyenMaiService,
            ILogger<KhuyenMaiController> logger)
        {
            _khuyenMaiService = khuyenMaiService;
            _logger = logger;
        }

        /// ✅ GET all
        [HttpGet]
        [Authorize(Roles = "Admin,NhanVien")]
        public async Task<IActionResult> GetAllKhuyenMai()
        {
            try
            {
                var result = await _khuyenMaiService.GetAllKhuyenMaiAsync();
                return Ok(ApiResponse.Ok(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching promotions");
                return StatusCode(500, ApiResponse.Fail("Lỗi server"));
            }
        }

        /// ✅ GET by id
        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin,NhanVien")]
        public async Task<IActionResult> GetKhuyenMaiById(int id)
        {
            try
            {
                var result = await _khuyenMaiService.GetKhuyenMaiByIdAsync(id);
                if (result == null)
                    return NotFound(ApiResponse.Fail("Không tìm thấy khuyến mãi"));

                return Ok(ApiResponse.Ok(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetch promotion {Id}", id);
                return StatusCode(500, ApiResponse.Fail("Lỗi server"));
            }
        }

        /// ✅ POST — Admin only
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateKhuyenMai([FromBody] KhuyenMaiCreateDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse.Fail("Dữ liệu không hợp lệ"));

            try
            {
                var success = await _khuyenMaiService.CreateKhuyenMaiAsync(request);
                if (!success)
                    return BadRequest(ApiResponse.Fail("Tạo khuyến mãi thất bại"));

                return StatusCode(201, ApiResponse.Ok(null, "Tạo khuyến mãi thành công"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating promotion");
                return StatusCode(500, ApiResponse.Fail("Lỗi server"));
            }
        }

        /// ✅ PUT — Admin only
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateKhuyenMai(int id, [FromBody] KhuyenMaiUpdateDTO request)
        {
            try
            {
                var success = await _khuyenMaiService.UpdateKhuyenMaiAsync(id, request);
                if (!success)
                    return NotFound(ApiResponse.Fail("Không tìm thấy khuyến mãi hoặc cập nhật thất bại"));

                return Ok(ApiResponse.Ok(null, "Cập nhật khuyến mãi thành công"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating promotion {Id}", id);
                return StatusCode(500, ApiResponse.Fail("Lỗi server"));
            }
        }

        /// ✅ DELETE — Admin only
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteKhuyenMai(int id)
        {
            try
            {
                var success = await _khuyenMaiService.DeleteKhuyenMaiAsync(id);
                if (!success)
                    return NotFound(ApiResponse.Fail("Không tìm thấy khuyến mãi hoặc xóa thất bại"));

                return Ok(ApiResponse.Ok(null, "Xóa khuyến mãi thành công"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting promotion {Id}", id);
                return StatusCode(500, ApiResponse.Fail("Lỗi server"));
            }
        }
    }
}
