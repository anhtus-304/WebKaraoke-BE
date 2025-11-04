using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebKaraoke.Business.Interfaces;
using WebKaraoke.DTO;

namespace WebKaraoke.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MonAnNuocUongController : ControllerBase
    {
        private readonly IMonAnService _monAnService;

        public MonAnNuocUongController(IMonAnService monAnService)
        {
            _monAnService = monAnService;
        }

        // GET: api/MonAnNuocUong
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _monAnService.GetAllMonAnAsync();
            return Ok(list);
        }

        // GET: api/MonAnNuocUong/danhmuc/{danhMuc}
        [HttpGet("danhmuc/{danhMuc}")]
        public async Task<IActionResult> GetByDanhMuc(string danhMuc)
        {
            var list = await _monAnService.GetMonAnByDanhMucAsync(danhMuc);
            return Ok(list);
        }

        // GET: api/MonAnNuocUong/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var monAn = await _monAnService.GetMonAnByIdAsync(id);
            if (monAn == null) return NotFound();
            return Ok(monAn);
        }

        // POST: api/MonAnNuocUong
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MonAnNuocUongCreateDTO dto)
        {
            var success = await _monAnService.AddMonAnAsync(dto);
            if (!success) return BadRequest("Thêm món ăn thất bại");
            return Ok("Thêm món ăn thành công");
        }

        // PUT: api/MonAnNuocUong/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] MonAnNuocUongUpdateDTO dto)
        {
            var success = await _monAnService.UpdateMonAnAsync(id, dto);
            if (!success) return BadRequest("Cập nhật món ăn thất bại");
            return Ok("Cập nhật thành công");
        }

        // DELETE: api/MonAnNuocUong/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _monAnService.DeleteMonAnAsync(id);
            if (!success) return NotFound("Món ăn không tồn tại");
            return Ok("Xóa món ăn thành công");
        }
    }
}
