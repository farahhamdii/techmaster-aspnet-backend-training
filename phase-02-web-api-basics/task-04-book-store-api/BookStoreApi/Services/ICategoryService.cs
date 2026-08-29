using BookStoreApi.DTOs;

namespace BookStoreApi.Services;

public interface ICategoryService
{
    IEnumerable<CategoryResponse> GetAll();

    CategoryResponse? GetById(int id);

    (bool Success, CategoryResponse? Category) Create(CreateCategoryRequest request);
}