namespace EFCoreModelingDrills.Entities;

public class PaymentSummary
{
    public int Id { get; set; }

    public int EnrollmentId { get; set; }

    public decimal TotalRequired { get; set; }

    public decimal TotalPaid { get; set; }

    public decimal RemainingAmount { get; set; }

    public string PaymentStatus { get; set; } = string.Empty;

    public Enrollment Enrollment { get; set; } = null!;
}