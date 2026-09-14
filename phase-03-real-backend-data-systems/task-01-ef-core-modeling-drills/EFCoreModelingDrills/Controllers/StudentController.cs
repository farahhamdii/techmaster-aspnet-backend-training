using EFCoreModelingDrills.Data;
using EFCoreModelingDrills.Entities;
using EFCoreModelingDrills.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFCoreModelingDrills.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IStudentService _studentService;

    public StudentController(
        AppDbContext context,
        IStudentService studentService)
    {
        _context = context;
        _studentService = studentService;
    }

    [HttpGet("{id}/tracks")]
    public async Task<IActionResult> GetStudentTracks(int id)
    {
        var student = await _context.Students
            .Include(s => s.Enrollments)
            .ThenInclude(e => e.Track)
            .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);

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

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStudent(int id)
    {
        var student = await _context.Students
            .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);

        if (student == null)
        {
            return NotFound("Not found");
        }

        student.IsDeleted = true;
        student.DeletedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPost]
    public async Task<IActionResult> CreateStudent(Student student)
    {
        var result = await _studentService.CreateAsync(student);

        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStudent(
        int id,
        Student student)
    {
        var result = await _studentService.UpdateAsync(id, student);

        if (result == null)
        {
            return NotFound("Student not found.");
        }

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetStudents()
    {
        var result = await _studentService.GetAllAsync();

        return Ok(result);
    }

    [HttpGet("paged")]
    public async Task<IActionResult> GetStudentsByPagination(
    int pageNumber = 1,
    int pageSize = 10)
    {
        if (pageNumber <= 0)
            return BadRequest("pageNumber must be greater than 0.");

        if (pageSize < 1 || pageSize > 50)
            return BadRequest("pageSize must be between 1 and 50.");

        var result = await _studentService.GetStudentsAsync(pageNumber,pageSize);

        return Ok(result);
    }
}