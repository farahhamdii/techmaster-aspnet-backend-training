
namespace TrainingCenter.Entities;

public class ActivityLog
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public string UserRole { get; set; } = string.Empty;

    public string Action { get; set; } = string.Empty;

    public string EntityName { get; set; } = string.Empty;

    public int? EntityId { get; set; }

    public string Description { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public string? IpAddress { get; set; }

    // Optional JSON string containing additional details.
    public string? Metadata { get; set; }

    public string? CorrelationId { get; set; }
}
