// Controllers/BaseController.cs
using Microsoft.AspNetCore.Mvc;
using WebKaraoke.API.Helpers;

namespace WebKaraoke.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController : ControllerBase
    {
        protected IActionResult Success(object? data = null, string? message = null)
        {
            return Ok(ApiResponse.Ok(data, message));
        }

        protected IActionResult CreatedSuccess(object? data = null, string? message = null)
        {
            return StatusCode(201, ApiResponse.Ok(data, message));
        }

        protected IActionResult Error(string message, int statusCode = 400)
        {
            return StatusCode(statusCode, ApiResponse.Fail(message));
        }

        protected IActionResult NotFoundError(string message = "Không tìm thấy dữ liệu")
        {
            return NotFound(ApiResponse.Fail(message));
        }

        protected IActionResult UnauthorizedError(string message = "Unauthorized")
        {
            return Unauthorized(ApiResponse.Fail(message));
        }
    }
}