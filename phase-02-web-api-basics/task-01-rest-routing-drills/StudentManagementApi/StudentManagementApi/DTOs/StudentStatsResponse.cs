namespace StudentManagementApi.DTOs;

public class StudentStatsResponse
{
    public int TotalStudents { get; set; }
    public int ActiveStudents { get; set; }
    public int InactiveStudents { get; set; }
    public Dictionary<string, int> CountByTrack { get; set; } = new();
}