using BookStoreApi.Models;

namespace BookStoreApi.Data;

public class InMemoryStore
{
    public List<Book> Books { get; } = new();

    public List<Author> Authors { get; } = new();

    public List<Category> Categories { get; } = new();

    public InMemoryStore()
    {
        Authors.AddRange(new[]
        {
            new Author
            {
                AuthorId = 1,
                FullName = "Robert C. Martin",
                Country = "USA",
                BirthDate = new DateTime(1952, 12, 5),
                CreatedAt = DateTime.UtcNow
            },
            new Author
            {
                AuthorId = 2,
                FullName = "Martin Fowler",
                Country = "UK",
                BirthDate = new DateTime(1963, 12, 18),
                CreatedAt = DateTime.UtcNow
            },
            new Author
            {
                AuthorId = 3,
                FullName = "Eric Evans",
                Country = "USA",
                BirthDate = new DateTime(1960, 7, 3),
                CreatedAt = DateTime.UtcNow
            }
        });

        Categories.AddRange(new[]
        {
            new Category
            {
                CategoryId = 1,
                Name = "Software Engineering",
                Description = "Books about software development and engineering.",
                IsActive = true
            },
            new Category
            {
                CategoryId = 2,
                Name = "Architecture",
                Description = "Books about software architecture and design.",
                IsActive = true
            },
            new Category
            {
                CategoryId = 3,
                Name = "Programming",
                Description = "Books about programming concepts and practices.",
                IsActive = true
            }
        });

        Books.AddRange(new[]
        {
            new Book
            {
                BookId = 1,
                Title = "Clean Code",
                ISBN = "9780132350884",
                PublishedYear = 2008,
                Price = 45.99m,
                StockQuantity = 10,
                AuthorId = 1,
                CategoryId = 1,
                IsAvailable = true,
                CreatedAt = DateTime.UtcNow,
                Author = Authors[0],
                Category = Categories[0]
            },
            new Book
            {
                BookId = 2,
                Title = "Refactoring",
                ISBN = "9780134757599",
                PublishedYear = 2018,
                Price = 55.99m,
                StockQuantity = 4,
                AuthorId = 2,
                CategoryId = 2,
                IsAvailable = true,
                CreatedAt = DateTime.UtcNow,
                Author = Authors[1],
                Category = Categories[1]
            },
            new Book
            {
                BookId = 3,
                Title = "Domain-Driven Design",
                ISBN = "9780321125217",
                PublishedYear = 2003,
                Price = 60.00m,
                StockQuantity = 0,
                AuthorId = 3,
                CategoryId = 2,
                IsAvailable = false,
                CreatedAt = DateTime.UtcNow,
                Author = Authors[2],
                Category = Categories[1]
            }
        });
    }
}