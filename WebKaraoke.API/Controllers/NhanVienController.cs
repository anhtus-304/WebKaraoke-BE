// Controllers/NhanVienController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebKaraoke.Business.Interfaces;
using WebKaraoke.DTO;

namespace WebKaraoke.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize(Roles = "Admin, NhanVien")]
    public class NhanVienController : BaseController
    {
        private readonly INhanVienService _service;

        public NhanVienController(INhanVienService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var list = await _service.GetAllNhanViensAsync();
                return Success(list, "Lấy danh sách nhân viên thành công");
            }
            catch (Exception ex)
            {
                return Error($"Lỗi server: {ex.Message}", 500);
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _service.GetNhanVienByIdAsync(id);
                return result is null ? NotFoundError("Không tìm thấy nhân viên") : Success(result);
            }
            catch (Exception ex)
            {
                return Error($"Lỗi server: {ex.Message}", 500);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] NhanVienCreateDTO dto)
        {
            try
            {
                var success = await _service.CreateNhanVienAsync(dto);
                return success ? CreatedSuccess(null, "Tạo nhân viên thành công") : Error("Tạo nhân viên thất bại");
            }
            catch (Exception ex)
            {
                return Error($"Lỗi server: {ex.Message}", 500);
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] NhanVienUpdateDTO dto)
        {
            try
            {
                var success = await _service.UpdateNhanVienAsync(id, dto);
                return success ? Success(null, "Cập nhật thành công") : NotFoundError("Không tìm thấy nhân viên");
            }
            catch (Exception ex)
            {
                return Error($"Lỗi server: {ex.Message}", 500);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var success = await _service.DeleteNhanVienAsync(id);
                return success ? Success(null, "Xóa thành công") : NotFoundError("Không tìm thấy nhân viên");
            }
            catch (Exception ex)
            {
                return Error($"Lỗi server: {ex.Message}", 500);
            }
        }
    }
}