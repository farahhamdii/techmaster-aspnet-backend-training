
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrainingCenter.Data;

namespace TrainingCenter.Controllers;

[ApiController]
[Route("api/admin")]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly TrainingCenterDbContext _context;
    public AdminController(TrainingCenterDbContext context)
    {
        _context = context;
    }

    [HttpGet("activity-logs")]
    public async Task<IActionResult> GetActivityLogs(
        [FromQuery] int? userId,
        [FromQuery] string? entityName,
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var query = _context.ActivityLogs.AsNoTracking();
        if (userId.HasValue)
        {
            query = query.Where(log => log.UserId == userId.Value);
        }
        if (!string.IsNullOrWhiteSpace(entityName))
        {
            query = query.Where(log => log.EntityName == entityName);
        }
        if (from.HasValue)
        {
            query = query.Where(log => log.CreatedAt >= from.Value);
        }
        if (to.HasValue)
        {
            query = query.Where(log => log.CreatedAt <= to.Value);
        }
        var totalCount = await query.CountAsync();
        var activityLogs = await query.OrderByDescending(log => log.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize).ToListAsync();

        return Ok(new
        {
            data = activityLogs,
            pageNumber,
            pageSize,
            totalCount,
            totalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
        });
    }
}
