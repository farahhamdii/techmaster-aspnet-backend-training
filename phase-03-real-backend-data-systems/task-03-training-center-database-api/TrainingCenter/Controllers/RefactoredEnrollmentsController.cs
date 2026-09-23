
using Microsoft.AspNetCore.Mvc;
using TrainingCenter.Common;
using TrainingCenter.DTOs;
using TrainingCenter.DTOs.Payment;
using TrainingCenter.Services;

namespace TrainingCenter.Controllers;

[ApiController]
[Route("api/refactored-enrollments")]
public class RefactoredEnrollmentsController : ControllerBase
{
    private readonly IEnrollmentService _enrollmentService;
    private readonly IPaymentService _paymentService;

    public RefactoredEnrollmentsController(
        IEnrollmentService enrollmentService,
        IPaymentService paymentService)
    {
        _enrollmentService = enrollmentService;
        _paymentService = paymentService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        string? status,
        int? trackId,
        int? studentId,
        string? paymentStatus,
        int pageNumber = 1,
        int pageSize = 10)
    {
        if (pageNumber < 1 || pageSize < 1)
        {
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = "Page number and page size must be greater than zero."
            });
        }

        var result = await _enrollmentService.GetPagedAsync(
            status,
            trackId,
            studentId,
            paymentStatus,
            pageNumber,
            pageSize);

        return Ok(new ApiResponse<PagedResult<EnrollmentDetailsResponse>>
        {
            Success = true,
            Message = "Enrollments retrieved successfully.",
            Data = result
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        if (id <= 0)
        {
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = "Invalid enrollment id."
            });
        }

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
    public async Task<IActionResult> Create([FromBody] CreateEnrollmentRequest request)
    {
        try
        {
            var enrollment = await _enrollmentService.CreateAsync(request);

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

    [HttpPost("pay")]
    public async Task<IActionResult> Pay(
        [FromBody] CreatePaymentRequest request)
    {
        if (request.Amount <= 0)
        {
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = "Payment amount must be greater than zero."
            });
        }

        try
        {
            var payment = await _paymentService.CreateAsync(request);

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Payment created successfully.",
                Data = payment
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

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (id <= 0)
        {
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = "Invalid enrollment id."
            });
        }

        var deleted = await _enrollmentService.SoftDeleteAsync(id);

        if (!deleted)
        {
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = "Enrollment not found."
            });
        }

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Enrollment deleted successfully."
        });
    }
}