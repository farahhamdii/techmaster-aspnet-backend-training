namespace EFCoreModelingDrills.DTOs;

public class EnrollmentPaymentSummaryDto
{
    public int EnrollmentId { get; set; }

    public int StudentId { get; set; }

    public int TrackId { get; set; }

    public decimal TotalRequired { get; set; }

    public decimal TotalPaid { get; set; }

    public decimal RemainingAmount { get; set; }

    public string PaymentStatus { get; set; } = string.Empty;
}