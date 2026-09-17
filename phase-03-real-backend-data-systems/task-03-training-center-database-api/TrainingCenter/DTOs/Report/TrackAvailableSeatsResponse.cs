namespace TrainingCenter.DTOs;

public class TrackAvailableSeatsResponse
{
    public int TrackId { get; set; }
    public string Title { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public int ActiveEnrollments { get; set; }
    public int RemainingSeats { get; set; }
}