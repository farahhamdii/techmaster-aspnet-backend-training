using Microsoft.EntityFrameworkCore;
using TrainingCenter.Data;
using TrainingCenter.DTOs;

namespace TrainingCenter.Services;
public class ReportService : IReportService
{
    private readonly TrainingCenterDbContext _context;
    public ReportService(TrainingCenterDbContext context)
    {
        _context = context;
    }

    public async Task<DashboardSummaryResponse>GetDashboardSummaryAsync()
    {
        var totalStudents = await _context.Students.CountAsync(s => !s.IsDeleted);
        var activeStudents = await _context.Students .CountAsync(s => !s.IsDeleted &&s.IsActive);
        var totalInstructors = await _context.Instructors .CountAsync();
        var totalTracks = await _context.TrainingTracks.CountAsync(t => !t.IsDeleted);
        var activeTracks = await _context.TrainingTracks.CountAsync(t =>!t.IsDeleted && t.Status == "Active");
        var totalEnrollments = await _context.Enrollments.CountAsync();
        var activeEnrollments = await _context.Enrollments .CountAsync(e => e.Status == "Active");
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

    public async Task<List<UnpaidEnrollmentResponse>>GetUnpaidEnrollmentsAsync()
    {
        return await _context.Enrollments
            .Select(e => new UnpaidEnrollmentResponse
            {
                EnrollmentId = e.EnrollmentId,
                StudentName = e.Student.FullName,
                TrackTitle = e.TrainingTrack.Title,
                TotalPaid = e.Payments.Where(p => p.PaymentStatus == "Paid").Sum(p => p.Amount),
                PaymentStatus = e.Payments.Any( p => p.PaymentStatus == "Paid") ? "Partially Paid"  : "Unpaid"
            })
            .Where(e =>e.PaymentStatus == "Unpaid"|| e.PaymentStatus == "Partially Paid")
            .ToListAsync();
    }

    public async Task<List<TrackCapacityResponse>>GetTrackCapacityAsync()
    {
        return await _context.TrainingTracks.Where(t => !t.IsDeleted)
            .Select(t => new TrackCapacityResponse
            {
                TrainingTrackId = t.TrainingTrackId,
                Title = t.Title,
                Capacity = t.Capacity,
                EnrolledStudents = t.Enrollments.Count(e => e.Status == "Active"),
                AvailableSeats = t.Capacity - t.Enrollments.Count(e => e.Status == "Active"),
                IsFull = t.Enrollments.Count(e => e.Status == "Active")>= t.Capacity
            })
            .ToListAsync();
    }

    public async Task<RevenueSummaryResponse>GetRevenueSummaryAsync()
    {
        var totalPayments =await _context.Payments.CountAsync();
        var paidPayments =await _context.Payments.CountAsync(p =>p.PaymentStatus == "Paid");
        var totalRevenue =await _context.Payments.Where(p => p.PaymentStatus == "Paid")
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
}

