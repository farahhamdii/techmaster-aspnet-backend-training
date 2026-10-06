
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TrainingCenter.Data;
using TrainingCenter.Entities;

namespace TrainingCenter.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly TrainingCenterDbContext _context;
        private readonly IJwtService _jwtService;
        private readonly IConfiguration _configuration;

        public RefreshTokenService(
            TrainingCenterDbContext context,
            IJwtService jwtService,
            IConfiguration configuration)
        {
            _context = context;
            _jwtService = jwtService;
            _configuration = configuration;
        }

        public async Task<string> CreateAsync(ApplicationUser user)
        {
            var rawToken = GenerateSecureToken();
            var tokenHash = HashToken(rawToken);
            var refreshTokenDays = _configuration.GetValue<int>("Jwt:RefreshTokenDays");

            if (refreshTokenDays <= 0)
            {
                refreshTokenDays = 7;
            }

            var refreshToken = new RefreshToken
            {
                TokenHash = tokenHash,
                ApplicationUserId = user.Id,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(refreshTokenDays)
            };

            _context.RefreshTokens.Add(refreshToken);

            await _context.SaveChangesAsync();

            return rawToken;
        }

        public async Task<(ApplicationUser User,string NewRefreshToken,string NewAccessToken, DateTime AccessTokenExpiresAt)>
            RefreshAsync(string refreshToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                throw new UnauthorizedAccessException("Invalid refresh token.");
            }

            var tokenHash = HashToken(refreshToken);
            var existingToken =await _context.RefreshTokens.Include(r => r.ApplicationUser)
                    .FirstOrDefaultAsync(r => r.TokenHash == tokenHash);

            if (existingToken == null)
            {
                throw new UnauthorizedAccessException("Invalid refresh token.");
            }

            if (existingToken.IsRevoked)
            {
                throw new UnauthorizedAccessException("Refresh token has been revoked.");
            }

            if (existingToken.IsExpired)
            {
                throw new UnauthorizedAccessException("Refresh token has expired.");
            }

            var user = existingToken.ApplicationUser;

            if (!user.IsActive)
            {
                throw new UnauthorizedAccessException("User account is inactive.");
            }

            var newRawToken = GenerateSecureToken();
            var newTokenHash = HashToken(newRawToken);
            var refreshTokenDays =_configuration.GetValue<int>("Jwt:RefreshTokenDays");

            if (refreshTokenDays <= 0)
            {
                refreshTokenDays = 7;
            }

            var newRefreshToken = new RefreshToken
            {
                TokenHash = newTokenHash,
                ApplicationUserId = user.Id,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(refreshTokenDays)
            };

            var (newAccessToken, accessTokenExpiresAt) =_jwtService.GenerateToken(user);

            existingToken.RevokedAt = DateTime.UtcNow;
            existingToken.ReplacedByTokenHash =newTokenHash;
            _context.RefreshTokens.Add(newRefreshToken);
            await _context.SaveChangesAsync();
            return (user, newRawToken,newAccessToken,accessTokenExpiresAt);
        }

        public async Task RevokeAsync(string refreshToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                return;
            }

            var tokenHash = HashToken(refreshToken);
            var existingToken =await _context.RefreshTokens
                    .FirstOrDefaultAsync( r => r.TokenHash == tokenHash);

            if (existingToken == null)
            {
                return;
            }

            if (!existingToken.IsRevoked)
            {
                existingToken.RevokedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        private static string GenerateSecureToken()
        {
            var randomBytes =RandomNumberGenerator.GetBytes(64);
            return Convert.ToBase64String(randomBytes);
        }

        private static string HashToken(string token)
        {
            var bytes =Encoding.UTF8.GetBytes(token);
            var hash = SHA256.HashData(bytes);
            return Convert.ToHexString(hash);
        }
    }
}