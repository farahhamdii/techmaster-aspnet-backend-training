using EFCoreModelingDrills.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFCoreModelingDrills.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InstructorController : ControllerBase
{
    private readonly AppDbContext _context;

    public InstructorController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("{id}/tracks")]
    public async Task<IActionResult> GetInstructorTracks(int id)
    {
        var instructor = await _context.Instructors
            .Include(i => i.Tracks)
            .FirstOrDefaultAsync(i => i.Id == id);

        if (instructor == null)
        {
            return NotFound("Instructor not found.");
        }

        return Ok(new
        {
            instructor.Id,
            instructor.FullName,
            Tracks = instructor.Tracks.Select(t => new
            {
                t.Id,
                t.Name
            })
        });
    }
}