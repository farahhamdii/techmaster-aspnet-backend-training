namespace BookStoreApi.DTOs;

public class AuthorResponse
{
    public int AuthorId { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Country { get; set; } = string.Empty;

    public DateTime? BirthDate { get; set; }

    public DateTime CreatedAt { get; set; }
}