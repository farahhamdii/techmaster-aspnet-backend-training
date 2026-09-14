namespace EFCoreModelingDrills.DTOs;

public class TrackDetailsDto
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public int InstructorId { get; set; }

    public string InstructorName { get; set; } = string.Empty;
}