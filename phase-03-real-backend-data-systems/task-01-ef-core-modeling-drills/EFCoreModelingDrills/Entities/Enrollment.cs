namespace EFCoreModelingDrills.Entities;

public class Enrollment
{
    public int Id { get; set; }

    public int StudentId { get; set; }

    public int TrackId { get; set; }

    public string Status { get; set; } = string.Empty;

    public DateTime EnrollmentDate { get; set; }

    public decimal? FinalGrade { get; set; }

    public Student Student { get; set; } = null!;

    public Track Track { get; set; } = null!;
    public PaymentSummary? PaymentSummary { get; set; }
}