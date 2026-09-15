
namespace TrainingCenter.DTOs;

public class DashboardSummaryResponse
{
    public int TotalStudents { get; set; }
    public int ActiveStudents { get; set; }
    public int TotalInstructors { get; set; }
    public int TotalTracks { get; set; }
    public int ActiveTracks { get; set; }
    public int TotalEnrollments { get; set; }
    public int ActiveEnrollments { get; set; }
    public decimal TotalRevenue { get; set; }
}

