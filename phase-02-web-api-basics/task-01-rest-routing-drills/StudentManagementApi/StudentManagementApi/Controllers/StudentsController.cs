using Microsoft.AspNetCore.Mvc;
using StudentManagementApi.DTOs;
using StudentManagementApi.Services;

namespace StudentManagementApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly IStudentService _studentService;

    public StudentsController(IStudentService studentService)
    {
        _studentService = studentService;
    }

    [HttpGet]
    public ActionResult<PagedResultResponse<StudentResponse>> GetAll(
        [FromQuery] string? search,
        [FromQuery] string? trackName,
        [FromQuery] bool? isActive,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        if (pageNumber < 1 || pageSize < 1)
        {
            return BadRequest(new { Message = "PageNumber and PageSize must be greater than zero." });
        }

        var result = _studentService.GetAll(search, trackName, isActive, pageNumber, pageSize);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public ActionResult<StudentResponse> GetById(int id)
    {
        var student = _studentService.GetById(id);
        if (student == null)
        {
            return NotFound(new { Message = $"Student with ID {id} was not found." });
        }

        return Ok(student);
    }

    [HttpGet("by-track/{trackName}")]
    public ActionResult<IEnumerable<StudentResponse>> GetByTrack(string trackName)
    {
        var students = _studentService.GetByTrack(trackName);
        return Ok(students);
    }

    [HttpGet("stats")]
    public ActionResult<StudentStatsResponse> GetStats()
    {
        var stats = _studentService.GetStats();
        return Ok(stats);
    }

    [HttpPost]
    public ActionResult<StudentResponse> Create([FromBody] CreateStudentRequest request)
    {
        if (_studentService.IsEmailExists(request.Email))
        {
            return BadRequest(new { Message = "A student with this email already exists." });
        }

        var createdStudent = _studentService.Create(request);
        return CreatedAtAction(nameof(GetById), new { id = createdStudent.StudentId }, createdStudent);
    }

    [HttpPut("{id:int}")]
    public ActionResult<StudentResponse> Update(int id, [FromBody] UpdateStudentRequest request)
    {
        if (_studentService.IsEmailExists(request.Email, excludeId: id))
        {
            return BadRequest(new { Message = "Another student with this email already exists." });
        }

        var updatedStudent = _studentService.Update(id, request);
        if (updatedStudent == null)
        {
            return NotFound(new { Message = $"Student with ID {id} was not found." });
        }

        return Ok(updatedStudent);
    }

    [HttpPatch("{id:int}/status")]
    public IActionResult UpdateStatus(int id, [FromBody] UpdateStudentStatusRequest request)
    {
        bool success = _studentService.UpdateStatus(id, request.IsActive);
        if (!success)
        {
            return NotFound(new { Message = $"Student with ID {id} was not found." });
        }

        return Ok(new { Message = $"Student status updated to {(request.IsActive ? "Active" : "Inactive")} successfully." });
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        bool success = _studentService.Delete(id);
        if (!success)
        {
            return NotFound(new { Message = $"Student with ID {id} was not found." });
        }

        return Ok(new { Message = $"Student with ID {id} has been deactivated successfully." });
    }
}