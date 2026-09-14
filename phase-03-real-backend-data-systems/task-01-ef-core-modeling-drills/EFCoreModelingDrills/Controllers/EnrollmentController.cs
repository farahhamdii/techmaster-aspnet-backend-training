using EFCoreModelingDrills.Data;
using EFCoreModelingDrills.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFCoreModelingDrills.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EnrollmentController : ControllerBase
{
    private readonly AppDbContext _context;

    public EnrollmentController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("{id}/payment-summary")]
    public async Task<IActionResult> GetPaymentSummary(int id)
    {
        var enrollment = await _context.Enrollments
            .Include(e => e.PaymentSummary)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (enrollment == null)
        {
            return NotFound("Enrollment not found.");
        }

        if (enrollment.PaymentSummary == null)
        {
            return NotFound("Payment summary not found.");
        }

        var result = new EnrollmentPaymentSummaryDto
        {
            EnrollmentId = enrollment.Id,
            StudentId = enrollment.StudentId,
            TrackId = enrollment.TrackId,
            TotalRequired = enrollment.PaymentSummary.TotalRequired,
            TotalPaid = enrollment.PaymentSummary.TotalPaid,
            RemainingAmount = enrollment.PaymentSummary.RemainingAmount,
            PaymentStatus = enrollment.PaymentSummary.PaymentStatus
        };

        return Ok(result);
    }
}