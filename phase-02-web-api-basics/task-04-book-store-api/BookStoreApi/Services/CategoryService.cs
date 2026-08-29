using BookStoreApi.Data;
using BookStoreApi.DTOs;
using BookStoreApi.Models;

namespace BookStoreApi.Services;

public class CategoryService : ICategoryService
{
    private readonly InMemoryStore _store;

    public CategoryService(InMemoryStore store)
    {
        _store = store;
    }

    public IEnumerable<CategoryResponse> GetAll()
    {
        return _store.Categories.Select(MapToResponse);
    }

    public CategoryResponse? GetById(int id)
    {
        var category = _store.Categories
            .FirstOrDefault(c => c.CategoryId == id);

        return category == null
            ? null
            : MapToResponse(category);
    }

    public (bool Success, CategoryResponse? Category) Create(
        CreateCategoryRequest request)
    {
        var categoryNameExists = _store.Categories
            .Any(c => c.Name.Equals(
                request.Name.Trim(),
                StringComparison.OrdinalIgnoreCase));

        if (categoryNameExists)
        {
            return (false, null);
        }

        var newId = _store.Categories.Count == 0
            ? 1
            : _store.Categories.Max(c => c.CategoryId) + 1;

        var category = new Category
        {
            CategoryId = newId,
            Name = request.Name.Trim(),
            Description = request.Description.Trim(),
            IsActive = request.IsActive
        };

        _store.Categories.Add(category);

        return (true, MapToResponse(category));
    }

    private static CategoryResponse MapToResponse(Category category)
    {
        return new CategoryResponse
        {
            CategoryId = category.CategoryId,
            Name = category.Name,
            Description = category.Description,
            IsActive = category.IsActive
        };
    }
}