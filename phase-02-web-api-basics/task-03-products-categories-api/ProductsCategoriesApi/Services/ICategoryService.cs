using ProductsCategoriesApi.DTOs;

namespace ProductsCategoriesApi.Services;

public interface ICategoryService
{
    IEnumerable<CategoryResponse> GetAll();

    CategoryResponse Create(CreateCategoryRequest request);
}