namespace TrainingCenter.DTOs;

public class InstructorWorkloadResponse
{
    public int InstructorId { get; set; }
    public string InstructorName { get; set; } = string.Empty;
    public int TrackCount { get; set; }
    public int ActiveStudents { get; set; }
}