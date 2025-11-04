using WebKaraoke.DTO;

namespace WebKaraoke.Business.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponse> LoginAsync(LoginRequest request);
        Task<bool> RegisterAsync(RegisterRequest request);
        Task<object> GetUserByIdAsync(int userId);
        Task<bool> ChangePasswordAsync(int userId, ChangePasswordRequest request);
        Task<bool> CheckUsernameExistsAsync(string username);
    //    
    }
}