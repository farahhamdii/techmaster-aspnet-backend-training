namespace EFCoreModelingDrills.Entities;

public class Student
{
    public int Id { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public bool IsActive { get; set; }
    public StudentProfile? Profile { get; set; }
    public ICollection<Enrollment> Enrollments { get; set; }
    = new List<Enrollment>();
}