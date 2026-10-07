
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
            return NotFound(new ApiResponse<StudentDetailsResponse>
            {
                Success = false,
                Message = "Student profile not found.",
                Data = null
            });
        }

        return Ok(new ApiResponse<StudentDetailsResponse>
        {
            Success = true,
            Message = "Student profile retrieved successfully.",
            Data = student
        });
    }



    // Admin only
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll(string? search, bool? isActive)
    {
        var students = await _studentService.GetAllAsync(search, isActive);
        return Ok(new ApiResponse<List<StudentListItemResponse>>
        {
            Success = true,
            Message = "Students retrieved successfully.",
            Data = students
        });
    }

    // Admin only
    [HttpGet("paged")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetStudents( int pageNumber = 1,int pageSize = 10)
    {
        try
        {
            var result = await _studentService .GetPagedStudentsAsync(pageNumber, pageSize);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }

    // Admin → can view any student
    // Student → can view own profile only
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
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = "Student not found."
            });
        }

        return Ok(new ApiResponse<StudentDetailsResponse>
        {
            Success = true,
            Message = "Student retrieved successfully.",
            Data = student
        });
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
                new ApiResponse<StudentListItemResponse>
                {
                    Success = true,
                    Message = "Student created successfully.",
                    Data = student
                });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = ex.Message
            });
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
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Student not found."
                });
            }

            return Ok(new ApiResponse<StudentListItemResponse>
            {
                Success = true,
                Message = "Student updated successfully.",
                Data = student
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = ex.Message
            });
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
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = "Student not found."
            });
        }

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Student deleted successfully."
        });
    }
}