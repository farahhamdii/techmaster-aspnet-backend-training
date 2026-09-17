
using Microsoft.AspNetCore.Mvc;
using TrainingCenter.Common;
using TrainingCenter.DTOs;
using TrainingCenter.Services;

namespace TrainingCenter.Controllers;

[ApiController]
[Route("api/enrollments")]
public class EnrollmentsController : ControllerBase
{
    private readonly IEnrollmentService _enrollmentService;

    public EnrollmentsController(IEnrollmentService enrollmentService)
    {
        _enrollmentService = enrollmentService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(string? status,int? trackId,int? studentId,
        string? paymentStatus)
    {
        var enrollments = await _enrollmentService.GetAllAsync( status,trackId,studentId,
            paymentStatus);

        return Ok(new ApiResponse<List<EnrollmentDetailsResponse>>
        {
            Success = true,
            Message = "Enrollments retrieved successfully.",
            Data = enrollments
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var enrollment = await _enrollmentService.GetByIdAsync(id);
        if (enrollment == null)
        {
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = "Enrollment not found."
            });
        }

        return Ok(new ApiResponse<EnrollmentDetailsResponse>
        {
            Success = true,
            Message = "Enrollment retrieved successfully.",
            Data = enrollment
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateEnrollmentRequest request)
    {
        try
        {
            var enrollment =await _enrollmentService.CreateAsync(request);
            return CreatedAtAction(
                nameof(GetById),
                new { id = enrollment.EnrollmentId },
                new ApiResponse<EnrollmentDetailsResponse>
                {
                    Success = true,
                    Message = "Enrollment created successfully.",
                    Data = enrollment
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

    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateStatus(int id,string status)
    {
        try
        {
            var enrollment = await _enrollmentService.UpdateStatusAsync(id,status);
            if (enrollment == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Enrollment not found."
                });
            }

            return Ok(new ApiResponse<EnrollmentDetailsResponse>
            {
                Success = true,
                Message = "Enrollment status updated successfully.",
                Data = enrollment
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

    [HttpGet("/api/students/{id}/enrollments")]
    public async Task<IActionResult> GetStudentEnrollments(int id)
    {
        var enrollments =await _enrollmentService.GetStudentEnrollmentsAsync(id);
        return Ok(new ApiResponse<List<EnrollmentDetailsResponse>>
        {
            Success = true,
            Message = "Student enrollments retrieved successfully.",
            Data = enrollments
        });
    }

    [HttpGet("/api/tracks/{id}/students")]
    public async Task<IActionResult> GetTrackStudents(int id)
    {
        var enrollments = await _enrollmentService.GetTrackStudentsAsync(id);
        return Ok(new ApiResponse<List<EnrollmentDetailsResponse>>
        {
            Success = true,
            Message = "Track students retrieved successfully.",
            Data = enrollments
        });
    }
}

