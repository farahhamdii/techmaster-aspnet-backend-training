using System.ComponentModel.DataAnnotations;

namespace StudentManagementApi.DTOs;

public class UpdateStudentRequest
{
    [Required(ErrorMessage = "Full Name is required.")]
    [StringLength(100, MinimumLength = 3)]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Phone Number is required.")]
    [Phone]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Track Name is required.")]
    public string TrackName { get; set; } = string.Empty;

    [Url]
    public string? GitHubProfileUrl { get; set; }

    [Url]
    public string? LinkedInProfileUrl { get; set; }
}