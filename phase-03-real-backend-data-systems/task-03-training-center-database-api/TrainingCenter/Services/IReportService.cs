
using TrainingCenter.DTOs;

namespace TrainingCenter.Services;

public interface IReportService
{
    Task<DashboardSummaryResponse> GetDashboardSummaryAsync();
    Task<List<UnpaidEnrollmentResponse>>GetUnpaidEnrollmentsAsync();
    Task<List<TrackCapacityResponse>> GetTrackCapacityAsync();
    Task<RevenueSummaryResponse> GetRevenueSummaryAsync();
    Task<List<TrackAvailableSeatsResponse>> GetTracksWithAvailableSeatsAsync();
    Task<List<RevenueByTrackResponse>> GetRevenueByTrackAsync();
    Task<List<TopTrackResponse>> GetTopTracksAsync(int top = 5);
    Task<List<InstructorWorkloadResponse>> GetInstructorWorkloadAsync();
    Task<List<StudentWithoutPaymentResponse>> GetStudentsWithoutPaymentsAsync();
}
