using TrainingCenter.Entities;

namespace TrainingCenter.Services
{
    public interface IJwtService
    {
        (string Token, DateTime ExpiresAt) GenerateToken(ApplicationUser user);
    }
}