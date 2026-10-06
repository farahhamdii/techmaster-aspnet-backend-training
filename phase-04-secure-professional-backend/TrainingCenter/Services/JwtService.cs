using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TrainingCenter.Entities;

namespace TrainingCenter.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public (string Token, DateTime ExpiresAt) GenerateToken(ApplicationUser user)
        {
            var jwtKey = _configuration["Jwt:Key"];

            if (string.IsNullOrWhiteSpace(jwtKey))
            {
                throw new InvalidOperationException("JWT key is not configured.");
            }

            var accessTokenMinutes = _configuration.GetValue<int>("Jwt:AccessTokenMinutes");

            if (accessTokenMinutes <= 0)
            {
                accessTokenMinutes = 15;
            }

            var expiresAt = DateTime.UtcNow.AddMinutes(accessTokenMinutes);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var key = new SymmetricSecurityKey( Encoding.UTF8.GetBytes(jwtKey));

            var credentials = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expiresAt,
                signingCredentials: credentials);

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

            return (accessToken, expiresAt);
        }
    }
}