
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using TrainingCenter.Data;
using TrainingCenter.Entities;

namespace TrainingCenter.Services;

public class AuditService : IAuditService
{
    private readonly TrainingCenterDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<AuditService> _logger;

    public AuditService(
        TrainingCenterDbContext context,
        IHttpContextAccessor httpContextAccessor,
        ILogger<AuditService> logger)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public async Task LogAsync(
        string action,
        string entityName,
        int? entityId,
        string description,
        int? userId = null,
        string? userRole = null,
        string? ipAddress = null,
        string? metadata = null,
        string? correlationId = null)
    {
        try
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if (userId == null &&int.TryParse(
                    httpContext?.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value, out var currentUserId))
            {
                userId = currentUserId;
            }

            if (string.IsNullOrWhiteSpace(userRole))
            {
                userRole = httpContext?.User.FindFirst(
                    System.Security.Claims.ClaimTypes.Role)?.Value;
            }

            if (string.IsNullOrWhiteSpace(userRole))
            {
                userRole = "System";
            }

            ipAddress ??= httpContext?.Connection.RemoteIpAddress?.ToString();

            correlationId ??= httpContext?.TraceIdentifier;

            if (!string.IsNullOrWhiteSpace(metadata))
            {
                using var document = JsonDocument.Parse(metadata);
            }

            var activityLog = new ActivityLog
            {
                UserId = userId,
                UserRole = userRole,
                Action = action,
                EntityName = entityName,
                EntityId = entityId,
                Description = description,
                CreatedAt = DateTime.UtcNow,
                IpAddress = ipAddress,
                Metadata = metadata,
                CorrelationId = correlationId
            };

            _context.ActivityLogs.Add(activityLog);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to save audit log. Action: {Action}, Entity: {EntityName}, EntityId: {EntityId}",
                action,
                entityName,
                entityId);

            // Audit failure must not hide the result of the main operation.
        }
    }
}