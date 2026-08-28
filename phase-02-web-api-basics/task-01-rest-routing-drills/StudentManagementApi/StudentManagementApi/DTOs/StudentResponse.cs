namespace StudentManagementApi.DTOs;

public class StudentResponse
{
    public int StudentId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string TrackName { get; set; } = string.Empty;
    public DateTime EnrollmentDate { get; set; }
    public bool IsActive { get; set; }
    public string? GitHubProfileUrl { get; set; }
    public string? LinkedInProfileUrl { get; set; }
}