
namespace TrainingCenter.DTOs;

public class UnpaidEnrollmentResponse
{
    public int EnrollmentId { get; set; }

    public string StudentName { get; set; } = string.Empty;

    public string TrackTitle { get; set; } = string.Empty;

    public decimal TotalPaid { get; set; }

    public string PaymentStatus { get; set; } = string.Empty;
}