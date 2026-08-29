namespace BookStoreApi.DTOs;

public class BookSummaryResponse
{
    public int TotalBooks { get; set; }

    public int AvailableBooks { get; set; }

    public int OutOfStockBooks { get; set; }

    public decimal TotalInventoryValue { get; set; }

    public List<CategoryBookCount> BooksPerCategory { get; set; } = new();

    public List<AuthorBookCount> BooksPerAuthor { get; set; } = new();
}

public class CategoryBookCount
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = string.Empty;

    public int BookCount { get; set; }
}

public class AuthorBookCount
{
    public int AuthorId { get; set; }

    public string AuthorName { get; set; } = string.Empty;

    public int BookCount { get; set; }
}