namespace TrainingCenter.Entities
{
    public class RefreshToken
    {
        public int Id { get; set; }

        public string TokenHash { get; set; } = string.Empty;

        public int ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; } = null!;

        public DateTime ExpiresAt { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? RevokedAt { get; set; }

        public string? ReplacedByTokenHash { get; set; }

        public bool IsRevoked => RevokedAt.HasValue;

        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;

        public bool IsActive => !IsRevoked && !IsExpired;
    }
}