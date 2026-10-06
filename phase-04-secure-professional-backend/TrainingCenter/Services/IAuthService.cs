using TrainingCenter.DTOs.Auth;

namespace TrainingCenter.Services
{
    public interface IAuthService
    {
        Task<AuthResponse> RegisterAsync(RegisterRequest request);
        Task<AuthResponse> LoginAsync(LoginRequest request);
        Task<CurrentUserResponse> GetCurrentUserAsync(int userId);
        Task<AuthResponse> RefreshTokenAsync(string refreshToken);
        Task ChangePasswordAsync(int userId,string currentPassword,string newPassword,string confirmNewPassword);
        Task LogoutAsync(string refreshToken);
    }
}