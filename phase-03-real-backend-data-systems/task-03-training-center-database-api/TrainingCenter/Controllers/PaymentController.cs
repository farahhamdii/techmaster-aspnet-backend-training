
using Microsoft.AspNetCore.Mvc;
using TrainingCenter.Common;
using TrainingCenter.DTOs;
using TrainingCenter.DTOs.Payment;
using TrainingCenter.Entities;
using TrainingCenter.Services;
using static System.Net.Mime.MediaTypeNames;

namespace TrainingCenter.Controllers;

[ApiController]
[Route("api/payments")]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentService _paymentService;
    public PaymentsController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }
    [HttpGet]
    public async Task<IActionResult> GetAll( DateTime? from, DateTime? to,string? status)
    {
        var payments = await _paymentService.GetAllAsync(from, to,status);
        return Ok(new ApiResponse<List<PaymentResponse>>
        {
            Success = true,
            Message = "Payments retrieved successfully.",
            Data = payments
        });
    }

    [HttpGet("{id}")]
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
    public async Task<IActionResult> Create( CreatePaymentRequest request)
    {
        try
        {
            var payment =await _paymentService.CreateAsync(request);
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
            return Conflict(new ApiResponse<object>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    [HttpGet("/api/enrollments/{id}/payments")]
    public async Task<IActionResult> GetEnrollmentPayments(int id)
    {
        try
        {
            var payments =await _paymentService.GetEnrollmentPaymentsAsync(id);

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
}

