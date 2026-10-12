
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrainingCenter.Common;
using TrainingCenter.DTOs;
using TrainingCenter.Services;

namespace TrainingCenter.Controllers;

[ApiController]
[Route("api/enrollments")]
[Authorize]
public class EnrollmentsController : ControllerBase
{
    private readonly IEnrollmentService _enrollmentService;

    public EnrollmentsController(IEnrollmentService enrollmentService)
    {
        _enrollmentService = enrollmentService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll(
        string? status,
        int? trackId,
        int? studentId,
        string? paymentStatus)
    {
        var enrollments = await _enrollmentService.GetAllAsync(
            status,
            trackId,
            studentId,
            paymentStatus);

        return Ok(
            ApiResponse<List<EnrollmentDetailsResponse>>.SuccessResponse(
                enrollments,"Enrollments retrieved successfully."));
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetById(int id)
    {
        var enrollment = await _enrollmentService.GetByIdAsync(id);

        if (enrollment == null)
        {
            return NotFound(ApiResponse<object>.ErrorResponse("Enrollment not found."));
        }

        return Ok(ApiResponse<EnrollmentDetailsResponse>.SuccessResponse(enrollment,"Enrollment retrieved successfully."));
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(CreateEnrollmentRequest request)
    {
        try
        {
            var enrollment = await _enrollmentService.CreateAsync(request);

            return CreatedAtAction(
                nameof(GetById),
                new { id = enrollment.EnrollmentId },
                ApiResponse<EnrollmentDetailsResponse>.SuccessResponse(enrollment,"Enrollment created successfully."));
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ApiResponse<object>.ErrorResponse(ex.Message));
        }
    }

    [HttpPut("{id}/status")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateStatus(int id,string status)
    {
        try
        {
            var enrollment = await _enrollmentService.UpdateStatusAsync(id,status);
            if (enrollment == null)
            {
                return NotFound(ApiResponse<object>.ErrorResponse("Enrollment not found."));
            }

            return Ok(ApiResponse<EnrollmentDetailsResponse>.SuccessResponse(
                    enrollment,"Enrollment status updated successfully."));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
        }
    }

    [HttpGet("/api/students/{id}/enrollments")]
    [Authorize(Roles = "Admin,Student")]
    public async Task<IActionResult> GetStudentEnrollments(int id)
    {
        if (User.IsInRole("Student"))
        {
            var studentIdClaim = User.FindFirst("StudentId")?.Value;

            if (!int.TryParse(studentIdClaim, out var studentId))
            {
                return Unauthorized();
            }

            if (studentId != id)
            {
                return Forbid();
            }
        }

        var enrollments =
            await _enrollmentService.GetStudentEnrollmentsAsync(id);

        return Ok(ApiResponse<List<EnrollmentDetailsResponse>>.SuccessResponse(enrollments,"Student enrollments retrieved successfully."));
    }

    [HttpGet("/api/tracks/{id}/students")]
    [Authorize(Roles = "Admin,Instructor")]
    public async Task<IActionResult> GetTrackStudents(int id)
    {
        if (User.IsInRole("Instructor"))
        {
            var instructorIdClaim =User.FindFirst("InstructorId")?.Value;
            if (!int.TryParse(instructorIdClaim, out var instructorId))
            {
                return Unauthorized();
            }

            var trackInstructorId = await _enrollmentService.GetTrackInstructorIdAsync(id);

            if (trackInstructorId == null)
            {
                return NotFound(ApiResponse<object>.ErrorResponse("Training track not found."));
            }

            if (trackInstructorId != instructorId)
            {
                return Forbid();
            }
        }

        var enrollments =await _enrollmentService.GetTrackStudentsAsync(id);

        return Ok(ApiResponse<List<EnrollmentDetailsResponse>>.SuccessResponse(enrollments,"Track students retrieved successfully."));
    }

    [HttpGet("/api/student/my-enrollments")]
    [Authorize(Roles = "Student")]
    public async Task<IActionResult> GetMyEnrollments()
    {
        var studentIdClaim = User.FindFirst("StudentId")?.Value;

        if (!int.TryParse(studentIdClaim, out var studentId))
        {
            return Unauthorized();
        }

        var enrollments = await _enrollmentService.GetStudentEnrollmentsAsync(studentId);

        return Ok(ApiResponse<List<EnrollmentDetailsResponse>>.SuccessResponse(
                enrollments,"My enrollments retrieved successfully."));
    }
}