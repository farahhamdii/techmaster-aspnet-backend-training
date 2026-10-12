
using TrainingCenter.Entities;

namespace TrainingCenter.Services;

public interface IAuditService
{
    Task LogAsync(
        string action,
        string entityName,
        int? entityId,
        string description,
        int? userId = null,
        string? userRole = null,
        string? ipAddress = null,
        string? metadata = null,
        string? correlationId = null);
}
