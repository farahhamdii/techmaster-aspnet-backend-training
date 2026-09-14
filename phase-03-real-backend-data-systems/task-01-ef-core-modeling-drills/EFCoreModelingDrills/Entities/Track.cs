namespace EFCoreModelingDrills.Entities;

public class Track
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public int InstructorId { get; set; }

    public Instructor Instructor { get; set; } = null!;
    public ICollection<Enrollment> Enrollments { get; set; }
    = new List<Enrollment>();
}