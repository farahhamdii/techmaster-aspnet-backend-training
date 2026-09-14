namespace EFCoreModelingDrills.Entities;

public class StudentProfile
{
    public int Id { get; set; }

    public string PhoneNumber { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public DateTime DateOfBirth { get; set; }

    public int StudentId { get; set; }
    //navigation
    public Student Student { get; set; } = null!;
}