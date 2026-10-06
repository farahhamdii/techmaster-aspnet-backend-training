
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrainingCenter.DTOs.Auth;
using TrainingCenter.Services;

namespace TrainingCenter.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(
            RegisterRequest request)
        {
            try
            {
                var result = await _authService.RegisterAsync(request);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(
            LoginRequest request)
        {
            try
            {
                var result =await _authService.LoginAsync(request);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userIdClaim = User.FindFirstValue( ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdClaim,out var userId))
            {
                return Unauthorized(new
                {
                    message = "Invalid user token."
                });
            }

            try
            {
                var result =await _authService.GetCurrentUserAsync(userId);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken(
            RefreshTokenRequest request)
        {
            try
            {
                var result =await _authService.RefreshTokenAsync( request.RefreshToken);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword( ChangePasswordRequest request)
        {
            var userIdClaim = User.FindFirstValue( ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdClaim,out var userId))
            {
                return Unauthorized(new
                {
                    message = "Invalid user token."
                });
            }

            try
            {
                await _authService.ChangePasswordAsync(
                    userId,
                    request.CurrentPassword,
                    request.NewPassword,
                    request.ConfirmNewPassword);

                return Ok(new
                {
                    message = "Password changed successfully."
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost("logout")]
        [AllowAnonymous]
        public async Task<IActionResult> Logout( LogoutRequest request)
        {
            await _authService.LogoutAsync( request.RefreshToken);

            return NoContent();
        }
    }
}