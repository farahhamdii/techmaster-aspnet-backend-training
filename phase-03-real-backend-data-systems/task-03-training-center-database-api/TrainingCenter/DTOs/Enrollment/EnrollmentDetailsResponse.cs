using TrainingCenter.DTOs.Payment;

namespace TrainingCenter.DTOs;

public class EnrollmentDetailsResponse
{
    public int EnrollmentId { get; set; }

    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;

    public int TrainingTrackId { get; set; }
    public string TrackTitle { get; set; } = string.Empty;

    public DateTime EnrollmentDate { get; set; }

    public string Status { get; set; } = string.Empty;

    public decimal ProgressPercentage { get; set; }

    public string? FinalResult { get; set; }

    public decimal TotalPaid { get; set; }

    public List<PaymentResponse> Payments { get; set; } = new();
}