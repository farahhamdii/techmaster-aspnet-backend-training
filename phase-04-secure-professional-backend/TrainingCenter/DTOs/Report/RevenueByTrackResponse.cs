
namespace TrainingCenter.DTOs;
public class RevenueByTrackResponse
{
    public int TrainingTrackId { get; set; }
    public string TrackTitle { get; set; } = string.Empty;
    public decimal TotalRevenue { get; set; }
    public int PaymentCount { get; set; }
}
