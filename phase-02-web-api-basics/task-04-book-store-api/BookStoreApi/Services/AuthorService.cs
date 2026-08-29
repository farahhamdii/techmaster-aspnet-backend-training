using BookStoreApi.Data;
using BookStoreApi.DTOs;
using BookStoreApi.Models;

namespace BookStoreApi.Services;

public class AuthorService : IAuthorService
{
    private readonly InMemoryStore _store;

    public AuthorService(InMemoryStore store)
    {
        _store = store;
    }

    public IEnumerable<AuthorResponse> GetAll()
    {
        return _store.Authors.Select(MapToResponse);
    }

    public AuthorResponse? GetById(int id)
    {
        var author = _store.Authors.FirstOrDefault(a => a.AuthorId == id);

        return author == null ? null : MapToResponse(author);
    }

    public AuthorResponse Create(CreateAuthorRequest request)
    {
        var newId = _store.Authors.Count == 0
            ? 1 : _store.Authors.Max(a => a.AuthorId) + 1;

        var author = new Author
        {
            AuthorId = newId,
            FullName = request.FullName.Trim(),
            Country = request.Country.Trim(),
            BirthDate = request.BirthDate,
            CreatedAt = DateTime.UtcNow
        };

        _store.Authors.Add(author);

        return MapToResponse(author);
    }

    private static AuthorResponse MapToResponse(Author author)
    {
        return new AuthorResponse
        {
            AuthorId = author.AuthorId,
            FullName = author.FullName,
            Country = author.Country,
            BirthDate = author.BirthDate,
            CreatedAt = author.CreatedAt
        };
    }
}