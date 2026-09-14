using EFCoreModelingDrills.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFCoreModelingDrills.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TrackController : ControllerBase
{
    private readonly AppDbContext _context;

    public TrackController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("{id}/students")]
    public async Task<IActionResult> GetTrackStudents(int id)
    {
        var track = await _context.Tracks
            .Include(t => t.Enrollments)
            .ThenInclude(e => e.Student)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (track == null)
        {
            return NotFound("Track not found.");
        }

        return Ok(track.Enrollments.Select(e => new
        {
            e.Student.Id,
            e.Student.FullName,
            e.Student.Email,
            e.Status,
            e.EnrollmentDate,
            e.FinalGrade
        }));
    }
}