namespace TrainingCenter.DTOs;

public class TopTrackResponse
{
    public int TrackId { get; set; }
    public string Title { get; set; } = string.Empty;
    public int ActiveEnrollments { get; set; }
}