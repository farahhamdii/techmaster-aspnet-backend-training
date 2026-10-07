
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrainingCenter.Common;
using TrainingCenter.DTOs;
using TrainingCenter.DTOs.Payment;
using TrainingCenter.Services;

namespace TrainingCenter.Controllers;

[ApiController]
[Route("api/payments")]
[Authorize]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentService _paymentService;
    private readonly IEnrollmentService _enrollmentService;

    public PaymentsController(
        IPaymentService paymentService,
        IEnrollmentService enrollmentService)
    {
        _paymentService = paymentService;
        _enrollmentService = enrollmentService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll( DateTime? from,DateTime? to, string? status)
    {
        var payments = await _paymentService.GetAllAsync(from,to,status);
        return Ok(new ApiResponse<List<PaymentResponse>>
        {
            Success = true,
            Message = "Payments retrieved successfully.",
            Data = payments
        });
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetById(int id)
    {
        var payment = await _paymentService.GetByIdAsync(id);
        if (payment == null)
        {
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = "Payment not found."
            });
        }

        return Ok(new ApiResponse<PaymentResponse>
        {
            Success = true,
            Message = "Payment retrieved successfully.",
            Data = payment
        });
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(CreatePaymentRequest request)
    {
        try
        {
            var payment = await _paymentService.CreateAsync(request);
            return CreatedAtAction(
                nameof(GetById),
                new { id = payment.PaymentId },
                new ApiResponse<PaymentResponse>
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

    [HttpPut("{id}/status")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateStatus(int id,string status)
    {
        try
        {
            var payment = await _paymentService.UpdateStatusAsync(id,status);
            if (payment == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Payment not found."
                });
            }

            return Ok(new ApiResponse<PaymentResponse>
            {
                Success = true,
                Message = "Payment status updated successfully.",
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

    [HttpGet("/api/enrollments/{id}/payments")]
    [Authorize(Roles = "Admin,Student")]
    public async Task<IActionResult> GetEnrollmentPayments(int id)
    {
        try
        {
            if (User.IsInRole("Student"))
            {
                var studentIdClaim = User.FindFirst("StudentId")?.Value;
                if (!int.TryParse(studentIdClaim, out var studentId))
                {
                    return Unauthorized();
                }

                var enrollmentStudentId =await _paymentService.GetEnrollmentStudentIdAsync(id);

                if (enrollmentStudentId == null)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Enrollment not found."
                    });
                }

                if (enrollmentStudentId != studentId)
                {
                    return Forbid();
                }
            }

            var payments =
                await _paymentService.GetEnrollmentPaymentsAsync(id);

            return Ok(new ApiResponse<List<PaymentResponse>>
            {
                Success = true,
                Message = "Enrollment payments retrieved successfully.",
                Data = payments
            });
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    [HttpGet("/api/student/my-payments")]
    [Authorize(Roles = "Student")]
    public async Task<IActionResult> GetMyPayments()
    {
        var studentIdClaim = User.FindFirst("StudentId")?.Value;
        if (!int.TryParse(studentIdClaim, out var studentId))
        {
            return Unauthorized();
        }
        var enrollments =await _enrollmentService.GetStudentEnrollmentsAsync(studentId);
        var payments = new List<PaymentResponse>();

        foreach (var enrollment in enrollments)
        {
            var enrollmentPayments = await _paymentService.GetEnrollmentPaymentsAsync(enrollment.EnrollmentId);
            payments.AddRange(enrollmentPayments);
        }

        return Ok(new ApiResponse<List<PaymentResponse>>
        {
            Success = true,
            Message = "My payments retrieved successfully.",
            Data = payments
        });
    }
}