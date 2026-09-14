namespace EFCoreModelingDrills.Entities;

public class Instructor
{
    public int Id { get; set; }

    public string FullName { get; set; } = string.Empty;

    public ICollection<Track> Tracks { get; set; } = new List<Track>();
}