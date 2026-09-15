namespace TrainingCenter.DTOs;

public class CreateInstructorRequest
{
    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Specialization { get; set; } = string.Empty;

    public string? Bio { get; set; }
}