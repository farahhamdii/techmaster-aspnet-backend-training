using EFCoreModelingDrills.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFCoreModelingDrills.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentController : ControllerBase
{
    private readonly AppDbContext _context;

    public StudentController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("{id}/tracks")]
    public async Task<IActionResult> GetStudentTracks(int id)
    {
        var student = await _context.Students
            .Include(s => s.Enrollments)
            .ThenInclude(e => e.Track)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (student == null)
        {
            return NotFound("Student not found.");
        }

        return Ok(student.Enrollments.Select(e => new
        {
            e.Track.Id,
            e.Track.Name,
            e.Status,
            e.EnrollmentDate,
            e.FinalGrade
        }));
    }
}