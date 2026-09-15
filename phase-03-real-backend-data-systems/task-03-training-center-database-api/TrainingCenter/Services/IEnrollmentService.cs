using TrainingCenter.DTOs;

namespace TrainingCenter.Services;

public interface IEnrollmentService
{
    Task<List<EnrollmentDetailsResponse>> GetAllAsync(string? status, int? trackId,int? studentId,
        string? paymentStatus);
    Task<EnrollmentDetailsResponse?> GetByIdAsync(int id);
    Task<EnrollmentDetailsResponse> CreateAsync(CreateEnrollmentRequest request);
    Task<EnrollmentDetailsResponse?> UpdateStatusAsync(int id, string status);
    Task<List<EnrollmentDetailsResponse>> GetStudentEnrollmentsAsync(int studentId);
    Task<List<EnrollmentDetailsResponse>> GetTrackStudentsAsync( int trackId);
}