namespace BookStoreApi.DTOs;

public class BookResponse
{
    public int BookId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string ISBN { get; set; } = string.Empty;

    public int PublishedYear { get; set; }

    public decimal Price { get; set; }

    public int StockQuantity { get; set; }

    public int AuthorId { get; set; }

    public string AuthorName { get; set; } = string.Empty;

    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = string.Empty;

    public bool IsAvailable { get; set; }

    public DateTime CreatedAt { get; set; }
}