using Azure;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using TrainingCenter.Common;
using TrainingCenter.Data;
using TrainingCenter.DTOs;
using TrainingCenter.Entities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TrainingCenter.Services;
public class ReportService : IReportService
{
    private readonly TrainingCenterDbContext _context;
    public ReportService(TrainingCenterDbContext context)
    {
        _context = context;
    }

    public async Task<DashboardSummaryResponse> GetDashboardSummaryAsync()
    {
        var totalStudents = await _context.Students.CountAsync(s => !s.IsDeleted);
        var activeStudents = await _context.Students.CountAsync(s => !s.IsDeleted && s.IsActive);
        var totalInstructors = await _context.Instructors.CountAsync();
        var totalTracks = await _context.TrainingTracks.CountAsync(t => !t.IsDeleted);
        var activeTracks = await _context.TrainingTracks.CountAsync(t => !t.IsDeleted && t.Status == "Active");
        var totalEnrollments = await _context.Enrollments.CountAsync();
        var activeEnrollments = await _context.Enrollments.CountAsync(e => e.Status == "Active");
        var totalRevenue = await _context.Payments.Where(p => p.PaymentStatus == "Paid")
            .SumAsync(p => (decimal?)p.Amount) ?? 0;

        return new DashboardSummaryResponse
        {
            TotalStudents = totalStudents,
            ActiveStudents = activeStudents,
            TotalInstructors = totalInstructors,
            TotalTracks = totalTracks,
            ActiveTracks = activeTracks,
            TotalEnrollments = totalEnrollments,
            ActiveEnrollments = activeEnrollments,
            TotalRevenue = totalRevenue
        };
    }

    public async Task<List<UnpaidEnrollmentResponse>> GetUnpaidEnrollmentsAsync()
    {
        return await _context.Enrollments
            .Select(e => new UnpaidEnrollmentResponse
            {
                EnrollmentId = e.EnrollmentId,
                StudentName = e.Student.FullName,
                TrackTitle = e.TrainingTrack.Title,
                TotalPaid = e.Payments.Where(p => p.PaymentStatus == "Paid").Sum(p => p.Amount),
                PaymentStatus = e.Payments.Any(p => p.PaymentStatus == "Paid") ? "Partially Paid" : "Unpaid"
            })
            .Where(e => e.PaymentStatus == "Unpaid" || e.PaymentStatus == "Partially Paid")
            .ToListAsync();
    }

    public async Task<List<TrackCapacityResponse>> GetTrackCapacityAsync()
    {
        return await _context.TrainingTracks.Where(t => !t.IsDeleted)
            .Select(t => new TrackCapacityResponse
            {
                TrainingTrackId = t.TrainingTrackId,
                Title = t.Title,
                Capacity = t.Capacity,
                EnrolledStudents = t.Enrollments.Count(e => e.Status == "Active"),
                AvailableSeats = t.Capacity - t.Enrollments.Count(e => e.Status == "Active"),
                IsFull = t.Enrollments.Count(e => e.Status == "Active") >= t.Capacity
            })
            .ToListAsync();
    }

    public async Task<RevenueSummaryResponse> GetRevenueSummaryAsync()
    {
        var totalPayments = await _context.Payments.CountAsync();
        var paidPayments = await _context.Payments.CountAsync(p => p.PaymentStatus == "Paid");
        var totalRevenue = await _context.Payments.Where(p => p.PaymentStatus == "Paid")
            .SumAsync(p => (decimal?)p.Amount) ?? 0;

        return new RevenueSummaryResponse
        {
            TotalRevenue = totalRevenue,
            TotalPayments = totalPayments,
            PaidPayments = paidPayments
        };
    }

    public async Task<List<RevenueByTrackResponse>> GetRevenueByTrackAsync()
    {
        return await _context.Payments.Where(p => p.PaymentStatus == "Paid")
            .GroupBy(p => new
            {
                p.Enrollment.TrainingTrackId,
                p.Enrollment.TrainingTrack.Title
            })
            .Select(g => new RevenueByTrackResponse
            {
                TrainingTrackId = g.Key.TrainingTrackId,
                TrackTitle = g.Key.Title,
                TotalRevenue = g.Sum(p => p.Amount),
                PaymentCount = g.Count()
            })
            .ToListAsync();
    }

    public async Task<List<TrackAvailableSeatsResponse>> GetTracksWithAvailableSeatsAsync()
    {
        return await _context.TrainingTracks
            .Select(t => new TrackAvailableSeatsResponse
            {
                TrackId = t.TrainingTrackId,
                Title = t.Title,
                Capacity = t.Capacity,

                ActiveEnrollments = t.Enrollments
                    .Count(e => e.Status == "Active"),

                RemainingSeats = t.Capacity -
                    t.Enrollments.Count(e => e.Status == "Active")
            })
            .Where(t => t.RemainingSeats > 0)
            .ToListAsync();
    }
    public async Task<List<TopTrackResponse>> GetTopTracksAsync(int top = 5)
    {
        if (top <= 0 || top > 100)
            throw new ArgumentException("Top must be between 1 and 100.");

        return await _context.Enrollments
            .Where(e => e.Status == "Active")
            .GroupBy(e => new
            {
                e.TrainingTrackId,
                e.TrainingTrack.Title
            })
            .Select(g => new TopTrackResponse
            {
                TrackId = g.Key.TrainingTrackId,
                Title = g.Key.Title,
                ActiveEnrollments = g.Count()
            })
            .OrderByDescending(x => x.ActiveEnrollments)
            .Take(top)
            .ToListAsync();
    }

    public async Task<List<InstructorWorkloadResponse>> GetInstructorWorkloadAsync()
    {
        return await _context.Instructors
     .Select(i => new InstructorWorkloadResponse
     {
         InstructorId = i.InstructorId,
         InstructorName = i.FullName,

         TrackCount = i.TrainingTracks.Count(),

         ActiveStudents = i.TrainingTracks
             .SelectMany(t => t.Enrollments)
             .Count(e => e.Status =="Active")
     })
     .ToListAsync();
    }
    public async Task<List<StudentWithoutPaymentResponse>>
    GetStudentsWithoutPaymentsAsync()
    {
        return await _context.Students
            .Where(s => s.Enrollments.Any(e =>
                (e.Status == "Active" ||
                 e.Status == "Pending")
                &&
                !e.Payments.Any()))
            .Select(s => new StudentWithoutPaymentResponse
            {
                StudentId = s.StudentId,
                Name = s.FullName,
                Email = s.Email
            })
            .ToListAsync();
    }
}
