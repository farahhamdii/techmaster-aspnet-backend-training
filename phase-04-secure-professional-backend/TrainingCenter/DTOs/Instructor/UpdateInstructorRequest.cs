namespace TrainingCenter.DTOs;

public class UpdateInstructorRequest
{
    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Specialization { get; set; } = string.Empty;

    public string? Bio { get; set; }

    public bool IsActive { get; set; }
}