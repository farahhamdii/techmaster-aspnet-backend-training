namespace BookStoreApi.Models;

public class Author
{
    public int AuthorId { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Country { get; set; } = string.Empty;

    public DateTime? BirthDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public List<Book> Books { get; set; } = new();
}