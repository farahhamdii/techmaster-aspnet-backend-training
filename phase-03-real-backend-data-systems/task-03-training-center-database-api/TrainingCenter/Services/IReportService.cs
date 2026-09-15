
using TrainingCenter.DTOs;

namespace TrainingCenter.Services;

public interface IReportService
{
    Task<DashboardSummaryResponse> GetDashboardSummaryAsync();
    Task<List<UnpaidEnrollmentResponse>>GetUnpaidEnrollmentsAsync();
    Task<List<TrackCapacityResponse>> GetTrackCapacityAsync();
    Task<RevenueSummaryResponse> GetRevenueSummaryAsync();
    Task<List<RevenueByTrackResponse>> GetRevenueByTrackAsync();
}
