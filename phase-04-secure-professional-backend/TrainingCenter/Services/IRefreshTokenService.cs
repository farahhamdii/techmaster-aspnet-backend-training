using TrainingCenter.Entities;

namespace TrainingCenter.Services
{
    public interface IRefreshTokenService
    {
        Task<string> CreateAsync(ApplicationUser user);

        Task<(ApplicationUser User,
            string NewRefreshToken,
            string NewAccessToken,
            DateTime AccessTokenExpiresAt)>
            RefreshAsync(string refreshToken);

        Task RevokeAsync(string refreshToken);
    }
}