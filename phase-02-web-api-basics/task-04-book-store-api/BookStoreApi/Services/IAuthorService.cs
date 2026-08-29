using BookStoreApi.DTOs;

namespace BookStoreApi.Services;

public interface IAuthorService
{
    IEnumerable<AuthorResponse> GetAll();

    AuthorResponse? GetById(int id);

    AuthorResponse Create(CreateAuthorRequest request);
}