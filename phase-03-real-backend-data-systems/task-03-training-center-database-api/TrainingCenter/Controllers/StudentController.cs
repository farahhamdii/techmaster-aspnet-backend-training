using Microsoft.AspNetCore.Mvc;
using TrainingCenter.Common;
using TrainingCenter.DTOs;
using TrainingCenter.Services;

namespace TrainingCenter.Api.Controllers;

[ApiController]
[Route("api/students")]
public class StudentController : ControllerBase
{
    private readonly IStudentService _studentService;

    public StudentController(IStudentService studentService)
    {
        _studentService = studentService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var students = await _studentService.GetAllAsync();

        return Ok(new ApiResponse<List<StudentListItemResponse>>
        {
            Success = true,
            Message = "Students retrieved successfully.",
            Data = students
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
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

    [HttpPost]
    public async Task<IActionResult> Create(CreateStudentRequest request)
    {
        try
        {
            var student = await _studentService.CreateAsync(request);

            return CreatedAtAction(
                nameof(GetById),
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
            return Conflict(new ApiResponse<object>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        int id,
        UpdateStudentRequest request)
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
            return Conflict(new ApiResponse<object>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    [HttpDelete("{id}")]
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