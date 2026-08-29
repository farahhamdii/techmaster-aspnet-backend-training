using ProductsCategoriesApi.Data;
using ProductsCategoriesApi.DTOs;
using ProductsCategoriesApi.Models;

namespace ProductsCategoriesApi.Services;

public class CategoryService : ICategoryService
{
    private readonly InMemoryStore _store;

    public CategoryService(InMemoryStore store)
    {
        _store = store;
    }

    public IEnumerable<CategoryResponse> GetAll()
    {
        return _store.Categories
            .Where(c => c.IsActive)
            .Select(c => new CategoryResponse
            {
                CategoryId = c.CategoryId,
                Name = c.Name,
                Description = c.Description,
                IsActive = c.IsActive,
                CreatedAt = c.CreatedAt,
                ProductCount = _store.Products.Count(p =>
                    p.CategoryId == c.CategoryId)
            })
            .ToList();
    }

    public CategoryResponse Create(CreateCategoryRequest request)
    {
        var categoryName = request.Name.Trim();

        var categoryExists = _store.Categories.Any(c =>
            c.Name.Equals(
                categoryName,
                StringComparison.OrdinalIgnoreCase));

        if (categoryExists)
        {
            throw new InvalidOperationException(
                "Category name already exists.");
        }

        var category = new Category
        {
            CategoryId = _store.NextCategoryId++,
            Name = categoryName,
            Description = request.Description.Trim(),
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        _store.Categories.Add(category);

        return new CategoryResponse
        {
            CategoryId = category.CategoryId,
            Name = category.Name,
            Description = category.Description,
            IsActive = category.IsActive,
            CreatedAt = category.CreatedAt,
            ProductCount = 0
        };
    }
}