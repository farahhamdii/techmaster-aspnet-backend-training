
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TrainingCenter.Data;
using TrainingCenter.DTOs.Auth;
using TrainingCenter.Entities;

namespace TrainingCenter.Services
{
    public class AuthService : IAuthService
    {
        private readonly TrainingCenterDbContext _context;
        private readonly IJwtService _jwtService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly PasswordHasher<ApplicationUser> _passwordHasher;

        public AuthService(
            TrainingCenterDbContext context,
            IJwtService jwtService,
            IRefreshTokenService refreshTokenService)
        {
            _context = context;
            _jwtService = jwtService;
            _refreshTokenService = refreshTokenService;
            _passwordHasher = new PasswordHasher<ApplicationUser>();
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.FullName))
                throw new ArgumentException("Full name is required.");

            if (string.IsNullOrWhiteSpace(request.Email))
                throw new ArgumentException("Email is required.");

            if (string.IsNullOrWhiteSpace(request.Password))
                throw new ArgumentException("Password is required.");

            if (request.Password != request.ConfirmPassword)
                throw new ArgumentException("Passwords do not match.");

            ValidatePasswordStrength(request.Password);
            var email = request.Email.Trim().ToLowerInvariant();

            var emailExists = await _context.Users.AnyAsync(u => u.Email == email);
            if (emailExists)
                throw new ArgumentException("An account with this email already exists.");

            if (!Enum.TryParse<UserRole>( request.Role, true,out var role))
            {
                throw new ArgumentException("Invalid role. Allowed roles are Student and Instructor.");
            }

            if (role == UserRole.Admin)
            {
                throw new ArgumentException("You cannot register as Admin.");
            }

            var user = new ApplicationUser
            {
                FullName = request.FullName.Trim(),
                Email = email,
                Role = role,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            user.PasswordHash = _passwordHasher.HashPassword( user, request.Password);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            var (accessToken, accessTokenExpiresAt) = _jwtService.GenerateToken(user);

            var refreshToken = await _refreshTokenService.CreateAsync(user);

            return new AuthResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresAt = accessTokenExpiresAt,
                UserId = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role.ToString()
            };
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email))
                throw new ArgumentException("Email is required.");
            if (string.IsNullOrWhiteSpace(request.Password))
                throw new ArgumentException( "Password is required.");
            var email = request.Email.Trim().ToLowerInvariant();

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
                throw new UnauthorizedAccessException("Invalid email or password.");

            if (!user.IsActive)
                throw new UnauthorizedAccessException("This account is inactive.");

            var passwordResult = _passwordHasher.VerifyHashedPassword( user,
                    user.PasswordHash,
                    request.Password);

            if (passwordResult==PasswordVerificationResult.Failed)
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            user.LastLoginAt = DateTime.UtcNow;

            var (accessToken, accessTokenExpiresAt) =_jwtService.GenerateToken(user);

            var refreshToken =await _refreshTokenService.CreateAsync(user);

            await _context.SaveChangesAsync();

            return new AuthResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresAt = accessTokenExpiresAt,
                UserId = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role.ToString()
            };
        }

        public async Task<CurrentUserResponse> GetCurrentUserAsync(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                throw new UnauthorizedAccessException("User not found.");
            return new CurrentUserResponse
            {
                UserId = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role.ToString(),
                LinkedStudentId = user.StudentId,
                LinkedInstructorId = user.InstructorId
            };
        }

        public async Task<AuthResponse> RefreshTokenAsync(string refreshToken)
        {
            var result = await _refreshTokenService.RefreshAsync(refreshToken);

            return new AuthResponse
            {
                AccessToken = result.NewAccessToken,
                RefreshToken = result.NewRefreshToken,
                ExpiresAt = result.AccessTokenExpiresAt,
                UserId = result.User.Id,
                FullName = result.User.FullName,
                Email = result.User.Email,
                Role = result.User.Role.ToString()
            };
        }

        public async Task ChangePasswordAsync(
            int userId,
            string currentPassword,
            string newPassword,
            string confirmNewPassword)
        {
            if (string.IsNullOrWhiteSpace(currentPassword))
                throw new ArgumentException("Current password is required.");

            if (string.IsNullOrWhiteSpace(newPassword))
                throw new ArgumentException("New password is required.");

            if (newPassword != confirmNewPassword)
                throw new ArgumentException("Passwords do not match.");

            ValidatePasswordStrength(newPassword);

            var user = await _context.Users .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                throw new UnauthorizedAccessException( "User not found.");

            var passwordResult = _passwordHasher.VerifyHashedPassword( user,user.PasswordHash, currentPassword);

            if (passwordResult ==PasswordVerificationResult.Failed)
            {
                throw new ArgumentException("Current password is incorrect.");
            }

            user.PasswordHash =_passwordHasher.HashPassword( user, newPassword);
            user.UpdatedAt = DateTime.UtcNow;

            var activeRefreshTokens =await _context.RefreshTokens
                    .Where(r =>r.ApplicationUserId == userId &&r.RevokedAt == null)
                    .ToListAsync();

            foreach (var token in activeRefreshTokens)
            {
                token.RevokedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
        }

        public async Task LogoutAsync(string refreshToken)
        {
            await _refreshTokenService.RevokeAsync(refreshToken);
        }

        private static void ValidatePasswordStrength(string password)
        {
            if (password.Length < 8)
            {
                throw new ArgumentException("Password must be at least 8 characters.");
            }

            if (!password.Any(char.IsUpper))
            {
                throw new ArgumentException("Password must contain at least one uppercase letter.");
            }

            if (!password.Any(char.IsLower))
            {
                throw new ArgumentException("Password must contain at least one lowercase letter.");
            }

            if (!password.Any(char.IsDigit))
            {
                throw new ArgumentException("Password must contain at least one digit.");
            }

            if (!password.Any(ch => !char.IsLetterOrDigit(ch)))
            {
                throw new ArgumentException("Password must contain at least one special character.");
            }
        }
    }
}
