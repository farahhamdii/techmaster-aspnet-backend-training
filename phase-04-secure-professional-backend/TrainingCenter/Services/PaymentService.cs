
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System;
using TrainingCenter.Data;
using TrainingCenter.DTOs;
using TrainingCenter.DTOs.Payment;
using TrainingCenter.Entities;
using static System.Net.Mime.MediaTypeNames;

namespace TrainingCenter.Services;

public class PaymentService : IPaymentService
{
    private readonly TrainingCenterDbContext _context;
    public PaymentService(TrainingCenterDbContext context)
    {
        _context = context;
    }
    public async Task<List<PaymentResponse>> GetAllAsync( DateTime? from,DateTime? to,string? status)
    {
        var query = _context.Payments .AsQueryable();
        if (from.HasValue)
        {
            query = query.Where(p =>p.PaymentDate >= from.Value);
        }

        if (to.HasValue)
        {
            query = query.Where(p =>p.PaymentDate <= to.Value);
        }

        if (!string.IsNullOrWhiteSpace(status))
        {
            query = query.Where(p =>p.PaymentStatus == status);
        }

        return await query
            .Select(p => new PaymentResponse
            {
                PaymentId = p.PaymentId,
                EnrollmentId = p.EnrollmentId,
                Amount = p.Amount,
                PaymentMethod = p.PaymentMethod,
                PaymentDate = p.PaymentDate,
                PaymentStatus = p.PaymentStatus,
                ReferenceNumber = p.ReferenceNumber,
                Notes = p.Notes
            })
            .ToListAsync();
    }

    public async Task<PaymentResponse?> GetByIdAsync(int id)
    {
        return await _context.Payments.Where(p => p.PaymentId == id)
            .Select(p => new PaymentResponse
            {
                PaymentId = p.PaymentId,
                EnrollmentId = p.EnrollmentId,
                Amount = p.Amount,
                PaymentMethod = p.PaymentMethod,
                PaymentDate = p.PaymentDate,
                PaymentStatus = p.PaymentStatus,
                ReferenceNumber = p.ReferenceNumber,
                Notes = p.Notes
            })
            .FirstOrDefaultAsync();
    }

    public async Task<PaymentResponse> CreateAsync( CreatePaymentRequest request)
    {
        if (request.Amount <= 0)
        {
            throw new InvalidOperationException("Payment amount must be greater than zero.");
        }

        if (string.IsNullOrWhiteSpace(request.PaymentMethod))
        {
            throw new InvalidOperationException("Payment method is required.");
        }

        if (string.IsNullOrWhiteSpace(request.ReferenceNumber))
        {
            throw new InvalidOperationException("Reference number is required.");
        }
        var enrollmentExists = await _context.Enrollments.AnyAsync(e =>e.EnrollmentId == request.EnrollmentId);
        if (!enrollmentExists)
        {
            throw new InvalidOperationException("Enrollment not found.");
        }

        var referenceExists = await _context.Payments .AnyAsync(p => p.ReferenceNumber == request.ReferenceNumber);

        if (referenceExists)
        {
            throw new InvalidOperationException("Reference number already exists.");
        }

        var validStatuses = new[]
        {
            "Pending",
            "Paid",
            "Failed",
            "Refunded"
        };

        if (!validStatuses.Contains(request.PaymentStatus))
        {
            throw new InvalidOperationException("Invalid payment status.");
        }

        var payment = new Payment
        {
            EnrollmentId = request.EnrollmentId,
            Amount = request.Amount,
            PaymentMethod = request.PaymentMethod,
            PaymentDate = DateTime.UtcNow,
            PaymentStatus = request.PaymentStatus,
            ReferenceNumber = request.ReferenceNumber,
            Notes = request.Notes
        };
        _context.Payments.Add(payment);
        await _context.SaveChangesAsync();
        return (await GetByIdAsync(payment.PaymentId))!;
    }

    public async Task<PaymentResponse?> UpdateStatusAsync(int id, string status)
    {
        var payment = await _context.Payments.FirstOrDefaultAsync(p => p.PaymentId == id);
        if (payment == null)
        {
            return null;
        }

        var validStatuses = new[]
        {
            "Pending",
            "Paid",
            "Failed",
            "Refunded"
        };

        if (!validStatuses.Contains(status))
        {
            throw new InvalidOperationException("Invalid payment status.");
        }

        payment.PaymentStatus = status;
        await _context.SaveChangesAsync();
        return await GetByIdAsync(id);
    }

    public async Task<List<PaymentResponse>> GetEnrollmentPaymentsAsync(int enrollmentId)
    {
        var enrollmentExists = await _context.Enrollments.AnyAsync(e =>e.EnrollmentId == enrollmentId);

        if (!enrollmentExists)
        {
            throw new InvalidOperationException("Enrollment not found.");
        }

        return await _context.Payments.Where(p => p.EnrollmentId == enrollmentId)
            .Select(p => new PaymentResponse
            {
                PaymentId = p.PaymentId,
                EnrollmentId = p.EnrollmentId,
                Amount = p.Amount,
                PaymentMethod = p.PaymentMethod,
                PaymentDate = p.PaymentDate,
                PaymentStatus = p.PaymentStatus,
                ReferenceNumber = p.ReferenceNumber,
                Notes = p.Notes
            })
            .ToListAsync();
    }
}


