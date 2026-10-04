namespace TrainingCenter.DTOs;
public class TrackCapacityResponse
{
    public int TrainingTrackId { get; set; }
    public string Title { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public int EnrolledStudents { get; set; }
    public int AvailableSeats { get; set; }
    public bool IsFull { get; set; }
}

