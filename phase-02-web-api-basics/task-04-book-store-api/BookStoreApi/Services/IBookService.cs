using BookStoreApi.DTOs;

namespace BookStoreApi.Services;

public interface IBookService
{
    IEnumerable<BookResponse> GetAll(
        string? search,
        int? categoryId,
        int? authorId,
        bool? isAvailable,
        int pageNumber,
        int pageSize);

    BookResponse? GetById(int id);

    (bool Success, string? Error, BookResponse? Book) Create(
        CreateBookRequest request);

    (bool Success, string? Error, BookResponse? Book) Update(
        int id,
        UpdateBookRequest request);

    bool Delete(int id);
    BookSummaryResponse GetSummary();
}