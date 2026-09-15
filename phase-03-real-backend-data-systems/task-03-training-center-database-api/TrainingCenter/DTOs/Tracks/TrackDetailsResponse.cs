namespace TrainingCenter.DTOs;

public class TrackDetailsResponse
{
    public int TrainingTrackId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Code { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string Level { get; set; } = string.Empty;

    public int Capacity { get; set; }

    public int EnrolledStudents { get; set; }

    public int AvailableSeats { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public string Status { get; set; } = string.Empty;

    public int InstructorId { get; set; }

    public string InstructorName { get; set; } = string.Empty;
}