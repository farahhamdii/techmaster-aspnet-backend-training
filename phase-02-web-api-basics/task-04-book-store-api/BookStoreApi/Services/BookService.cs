using BookStoreApi.Data;
using BookStoreApi.DTOs;
using BookStoreApi.Models;

namespace BookStoreApi.Services;

public class BookService : IBookService
{
    private readonly InMemoryStore _store;

    public BookService(InMemoryStore store)
    {
        _store = store;
    }

    public IEnumerable<BookResponse> GetAll(
        string? search,
        int? categoryId,
        int? authorId,
        bool? isAvailable,
        int pageNumber,
        int pageSize)
    {
        var query = _store.Books.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            search = search.Trim();

            query = query.Where(b =>
                b.Title.Contains(
                    search,
                    StringComparison.OrdinalIgnoreCase)
                ||
                b.ISBN.Contains(
                    search,
                    StringComparison.OrdinalIgnoreCase));
        }

        if (categoryId.HasValue)
        {
            query = query.Where(b =>
                b.CategoryId == categoryId.Value);
        }

        if (authorId.HasValue)
        {
            query = query.Where(b =>
                b.AuthorId == authorId.Value);
        }

        if (isAvailable.HasValue)
        {
            query = query.Where(b =>
                b.IsAvailable == isAvailable.Value);
        }

        query = query
            .OrderBy(b => b.BookId)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);

        return query.Select(MapToResponse);
    }

    public BookResponse? GetById(int id)
    {
        var book = _store.Books
            .FirstOrDefault(b => b.BookId == id);

        return book == null
            ? null
            : MapToResponse(book);
    }
    public (bool Success, string? Error, BookResponse? Book) Create(
    CreateBookRequest request)
    {
        var author = _store.Authors
            .FirstOrDefault(a => a.AuthorId == request.AuthorId);

        if (author == null)
        {
            return (false, "AUTHOR_NOT_FOUND", null);
        }

        var category = _store.Categories
            .FirstOrDefault(c => c.CategoryId == request.CategoryId);

        if (category == null)
        {
            return (false, "CATEGORY_NOT_FOUND", null);
        }

        if (!category.IsActive)
        {
            return (false, "CATEGORY_INACTIVE", null);
        }

        var isbnExists = _store.Books
            .Any(b => b.ISBN.Equals(
                request.ISBN.Trim(),
                StringComparison.OrdinalIgnoreCase));

        if (isbnExists)
        {
            return (false, "ISBN_EXISTS", null);
        }

        var newId = _store.Books.Count == 0
            ? 1
            : _store.Books.Max(b => b.BookId) + 1;

        var book = new Book
        {
            BookId = newId,
            Title = request.Title.Trim(),
            ISBN = request.ISBN.Trim(),
            PublishedYear = request.PublishedYear,
            Price = request.Price,
            StockQuantity = request.StockQuantity,
            AuthorId = request.AuthorId,
            CategoryId = request.CategoryId,
            IsAvailable = request.StockQuantity > 0,
            CreatedAt = DateTime.UtcNow,
            Author = author,
            Category = category
        };

        _store.Books.Add(book);

        return (true, null, MapToResponse(book));
    }
    public (bool Success, string? Error, BookResponse? Book) Update(
    int id,
    UpdateBookRequest request)
    {
        var book = _store.Books
            .FirstOrDefault(b => b.BookId == id);

        if (book == null)
        {
            return (false, "BOOK_NOT_FOUND", null);
        }

        var author = _store.Authors
            .FirstOrDefault(a => a.AuthorId == request.AuthorId);

        if (author == null)
        {
            return (false, "AUTHOR_NOT_FOUND", null);
        }

        var category = _store.Categories
            .FirstOrDefault(c => c.CategoryId == request.CategoryId);

        if (category == null)
        {
            return (false, "CATEGORY_NOT_FOUND", null);
        }

        if (!category.IsActive)
        {
            return (false, "CATEGORY_INACTIVE", null);
        }

        var isbnExists = _store.Books.Any(b =>
            b.BookId != id &&
            b.ISBN.Equals(
                request.ISBN.Trim(),
                StringComparison.OrdinalIgnoreCase));

        if (isbnExists)
        {
            return (false, "ISBN_EXISTS", null);
        }

        book.Title = request.Title.Trim();
        book.ISBN = request.ISBN.Trim();
        book.PublishedYear = request.PublishedYear;
        book.Price = request.Price;
        book.StockQuantity = request.StockQuantity;
        book.AuthorId = request.AuthorId;
        book.CategoryId = request.CategoryId;
        book.IsAvailable = request.IsAvailable;

        book.Author = author;
        book.Category = category;

        return (true, null, MapToResponse(book));
    }
    public bool Delete(int id)
    {
        var book = _store.Books
            .FirstOrDefault(b => b.BookId == id);

        if (book == null)
        {
            return false;
        }

        book.IsAvailable = false;

        return true;
    }
    public BookSummaryResponse GetSummary()
    {
        var totalBooks = _store.Books.Count;

        var availableBooks = _store.Books
            .Count(b => b.IsAvailable);

        var outOfStockBooks = _store.Books
            .Count(b => b.StockQuantity == 0);

        var totalInventoryValue = _store.Books
            .Sum(b => b.Price * b.StockQuantity);

        var booksPerCategory = _store.Categories
            .Select(category => new CategoryBookCount
            {
                CategoryId = category.CategoryId,
                CategoryName = category.Name,
                BookCount = _store.Books.Count(
                    b => b.CategoryId == category.CategoryId)
            })
            .ToList();

        var booksPerAuthor = _store.Authors
            .Select(author => new AuthorBookCount
            {
                AuthorId = author.AuthorId,
                AuthorName = author.FullName,
                BookCount = _store.Books.Count(
                    b => b.AuthorId == author.AuthorId)
            })
            .ToList();

        return new BookSummaryResponse
        {
            TotalBooks = totalBooks,
            AvailableBooks = availableBooks,
            OutOfStockBooks = outOfStockBooks,
            TotalInventoryValue = totalInventoryValue,
            BooksPerCategory = booksPerCategory,
            BooksPerAuthor = booksPerAuthor
        };
    }
    private static BookResponse MapToResponse(Book book)
    {
        return new BookResponse
        {
            BookId = book.BookId,
            Title = book.Title,
            ISBN = book.ISBN,
            PublishedYear = book.PublishedYear,
            Price = book.Price,
            StockQuantity = book.StockQuantity,
            AuthorId = book.AuthorId,
            AuthorName = book.Author?.FullName ?? string.Empty,
            CategoryId = book.CategoryId,
            CategoryName = book.Category?.Name ?? string.Empty,
            IsAvailable = book.IsAvailable,
            CreatedAt = book.CreatedAt
        };
    }
}