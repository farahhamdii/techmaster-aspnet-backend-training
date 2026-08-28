using System.ComponentModel.DataAnnotations;

namespace StudentManagementApi.DTOs;

public class CreateStudentRequest
{
    [Required(ErrorMessage = "Full Name is required.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 100 characters.")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Phone Number is required.")]
    [Phone(ErrorMessage = "Invalid phone number format.")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Track Name is required.")]
    public string TrackName { get; set; } = string.Empty;

    [Url(ErrorMessage = "Invalid GitHub URL format.")]
    public string? GitHubProfileUrl { get; set; }

    [Url(ErrorMessage = "Invalid LinkedIn URL format.")]
    public string? LinkedInProfileUrl { get; set; }
}