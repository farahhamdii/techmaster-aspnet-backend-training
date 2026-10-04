namespace TrainingCenter.DTOs;

public class StudentDetailsResponse
{
    public int StudentId { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string? PhoneNumber { get; set; }

    public bool IsActive { get; set; }

    public int TotalEnrollments { get; set; }

    public int ActiveEnrollments { get; set; }
}