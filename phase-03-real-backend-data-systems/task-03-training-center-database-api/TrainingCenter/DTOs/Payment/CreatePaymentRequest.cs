
namespace TrainingCenter.DTOs;

public class CreatePaymentRequest
{
    public int EnrollmentId { get; set; }

    public decimal Amount { get; set; }

    public string PaymentMethod { get; set; } = string.Empty;

    public string PaymentStatus { get; set; } = string.Empty;

    public string ReferenceNumber { get; set; } = string.Empty;

    public string? Notes { get; set; }
}
