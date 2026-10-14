
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;
        private readonly ILogger<AuthService> _logger;
        private readonly IAuditService _auditService;

        public AuthService(
            TrainingCenterDbContext context,
            IJwtService jwtService,
            IRefreshTokenService refreshTokenService,
            IPasswordHasher<ApplicationUser> passwordHasher,
            ILogger<AuthService> logger,
            IAuditService auditService)
        {
            _context = context;
            _jwtService = jwtService;
            _refreshTokenService = refreshTokenService;
            _passwordHasher = passwordHasher;
            _logger = logger;
            _auditService = auditService;
        }

        public async Task<AuthResponse> RegisterAsync(
            RegisterRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.FullName))
                throw new ArgumentException("Full name is required.");

            var fullName = request.FullName.Trim();

            if (fullName.Length < 2 || fullName.Length > 100)
                throw new ArgumentException(
                    "Full name must be between 2 and 100 characters.");

            if (string.IsNullOrWhiteSpace(request.Email))
                throw new ArgumentException("Email is required.");

            var email = request.Email.Trim().ToLowerInvariant();

            if (email.Length > 256 ||
                !new EmailAddressAttribute().IsValid(email))
            {
                throw new ArgumentException("Invalid email address.");
            }

            if (string.IsNullOrWhiteSpace(request.Password))
                throw new ArgumentException("Password is required.");

            if (request.Password.Length > 128)
                throw new ArgumentException(
                    "Password cannot exceed 128 characters.");

            if (request.Password != request.ConfirmPassword)
                throw new ArgumentException("Passwords do not match.");

            ValidatePasswordStrength(request.Password);

            var emailExists = await _context.Users
                .AnyAsync(u => u.Email == email);

            if (emailExists)
            {
                throw new ArgumentException("An account with this email already exists.");
            }

            if (!Enum.TryParse<UserRole>(
                    request.Role,
                    true,
                    out var role) ||
                !Enum.IsDefined(role))
            {
                throw new ArgumentException("Invalid role.");
            }

            // Public registration is restricted to Student accounts.
            // Instructor and Admin accounts must be created through
            // authorized administrative workflows.
            if (role != UserRole.Student)
            {
                throw new ForbiddenAccessException( "You are not allowed to register with this role.");
            }

            var student = new Student
            {
                FullName = fullName,
                Email = email,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            var user = new ApplicationUser
            {
                FullName = fullName,
                Email = email,
                Role = UserRole.Student,
                StudentId = student.StudentId,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            user.PasswordHash = _passwordHasher.HashPassword(
                user,
                request.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var (accessToken, accessTokenExpiresAt) =
                _jwtService.GenerateToken(user);

            var refreshToken =
                await _refreshTokenService.CreateAsync(user);

            await _auditService.LogAsync(
                action: "UserRegistered",
                entityName: "ApplicationUser",
                entityId: user.Id,
                description: "A new user registered successfully.",
                userId: user.Id,
                userRole: user.Role.ToString());

            _logger.LogInformation(
                "User registered successfully. UserId: {UserId}, Role: {Role}",
                user.Id,
                user.Role);

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

        public async Task<AuthResponse> LoginAsync( LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) ||!new EmailAddressAttribute().IsValid(
                    request.Email.Trim()))
            {
                throw new ArgumentException("Invalid email address.");
            }

            if (string.IsNullOrWhiteSpace(request.Password))
                throw new ArgumentException("Password is required.");

            var email = request.Email.Trim().ToLowerInvariant();
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                _logger.LogWarning("Login failed: invalid credentials.");
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            if (!user.IsActive)
            {
                _logger.LogWarning("Login failed: inactive account. UserId: {UserId}",user.Id);
                throw new UnauthorizedAccessException( "This account is inactive.");
            }

            var passwordResult =
                _passwordHasher.VerifyHashedPassword(
                    user,
                    user.PasswordHash,
                    request.Password);

            if (passwordResult == PasswordVerificationResult.Failed)
            {
                _logger.LogWarning("Login failed: invalid credentials. UserId: {UserId}",user.Id);
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            if (passwordResult ==
                PasswordVerificationResult.SuccessRehashNeeded)
            {
                user.PasswordHash = _passwordHasher.HashPassword(
                    user,
                    request.Password);
            }

            user.LastLoginAt = DateTime.UtcNow;

            var (accessToken, accessTokenExpiresAt) =
                _jwtService.GenerateToken(user);

            var refreshToken =
                await _refreshTokenService.CreateAsync(user);

            await _context.SaveChangesAsync();

            await _auditService.LogAsync(
                action: "UserLoggedIn",
                entityName: "ApplicationUser",
                entityId: user.Id,
                description: "User logged in successfully.",
                userId: user.Id,
                userRole: user.Role.ToString());

            _logger.LogInformation("Login successful. UserId: {UserId}, Role: {Role}",
                user.Id,
                user.Role);

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
            if (user == null || !user.IsActive)
            {
                throw new UnauthorizedAccessException( "User not found or inactive.");
            }
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

        public async Task<AuthResponse> RefreshTokenAsync(
            string refreshToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                throw new UnauthorizedAccessException("Invalid refresh token.");
            }

            var result =
                await _refreshTokenService.RefreshAsync(refreshToken);

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

            if (newPassword.Length > 128)
                throw new ArgumentException("Password cannot exceed 128 characters.");

            if (newPassword != confirmNewPassword)
                throw new ArgumentException("Passwords do not match.");
            ValidatePasswordStrength(newPassword);

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null || !user.IsActive)
            {
                throw new UnauthorizedAccessException("User not found or inactive.");
            }

            var passwordResult = _passwordHasher.VerifyHashedPassword(user,user.PasswordHash, currentPassword);

            if (passwordResult == PasswordVerificationResult.Failed)
            {
                throw new UnauthorizedAccessException("Current password is incorrect.");
            }

            user.PasswordHash = _passwordHasher.HashPassword(user,newPassword);

            user.UpdatedAt = DateTime.UtcNow;

            var activeRefreshTokens = await _context.RefreshTokens
                .Where(r =>r.ApplicationUserId == userId && r.RevokedAt == null)
                .ToListAsync();

            foreach (var token in activeRefreshTokens)
            {
                token.RevokedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();

            await _auditService.LogAsync(
                action: "PasswordChanged",
                entityName: "ApplicationUser",
                entityId: user.Id,
                description: "User changed their password.",
                userId: user.Id,
                userRole: user.Role.ToString());

            _logger.LogInformation("Password changed successfully. UserId: {UserId}", userId);
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
