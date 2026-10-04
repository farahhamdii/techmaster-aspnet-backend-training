namespace TrainingCenter.DTOs;

public class InstructorListItemResponse
{
    public int InstructorId { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Specialization { get; set; } = string.Empty;

    public bool IsActive { get; set; }
}