
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TrainingCenter.Common;
using TrainingCenter.DTOs;
using TrainingCenter.Services;

namespace TrainingCenter.Api.Controllers;

[ApiController]
[Route("api/students")]
[Authorize]
public class StudentController : ControllerBase
{
    private readonly IStudentService _studentService;

    public StudentController(IStudentService studentService)
    {
        _studentService = studentService;
    }

    // Student only: Get own profile
    [HttpGet("/api/student/me")]
    [Authorize(Roles = "Student")]
    public async Task<IActionResult> GetMyProfile()
    {
        var studentIdClaim = User.FindFirst("StudentId")?.Value;

        if (!int.TryParse(studentIdClaim, out var studentId))
        {
            return Unauthorized();
        }

        var student = await _studentService.GetByIdAsync(studentId);

        if (student == null)
        {
            return NotFound(ApiResponse<StudentDetailsResponse>.ErrorResponse( "Student profile not found."));
        }

        return Ok(ApiResponse<StudentDetailsResponse>.SuccessResponse(
                student,"Student profile retrieved successfully."));
    }

    // Admin only
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll(string? search, bool? isActive)
    {
        var students = await _studentService.GetAllAsync(search, isActive);
        return Ok(ApiResponse<List<StudentListItemResponse>>.SuccessResponse(
                students,"Students retrieved successfully."));
    }

    // Admin only
    [HttpGet("paged")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetStudents( int pageNumber = 1,int pageSize = 10)
    {
        try
        {
            var result = await _studentService.GetPagedStudentsAsync(pageNumber,pageSize);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
        }
    }

    // Admin can view any student.
    // Student can view own profile only.
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Student")]
    public async Task<IActionResult> GetById(int id)
    {
        if (User.IsInRole("Student"))
        {
            var studentIdClaim = User.FindFirstValue("StudentId");
            if (!int.TryParse(studentIdClaim, out var currentStudentId))
            {
                return Unauthorized();
            }
            if (currentStudentId != id)
            {
                return Forbid();
            }
        }

        var student = await _studentService.GetByIdAsync(id);
        if (student == null)
        {
            return NotFound(ApiResponse<object>.ErrorResponse( "Student not found."));
        }

        return Ok(ApiResponse<StudentDetailsResponse>.SuccessResponse(
                student,"Student retrieved successfully."));
    }

    // Admin only
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(CreateStudentRequest request)
    {
        try
        {
            var student = await _studentService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById),
                new { id = student.StudentId },
                ApiResponse<StudentListItemResponse>.SuccessResponse(student, "Student created successfully."));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
        }
    }

    // Admin only
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id,UpdateStudentRequest request)
    {
        try
        {
            var student = await _studentService.UpdateAsync(id, request);
            if (student == null)
            {
                return NotFound(ApiResponse<object>.ErrorResponse("Student not found."));
            }

            return Ok(ApiResponse<StudentListItemResponse>.SuccessResponse(
                    student,"Student updated successfully."));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
        }
    }

    // Admin only
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _studentService.DeleteAsync(id);
        if (!deleted)
        {
            return NotFound(ApiResponse<object>.ErrorResponse("Student not found."));
        }

        return Ok(ApiResponse<object>.SuccessResponse( null, "Student deleted successfully."));
    }
}
