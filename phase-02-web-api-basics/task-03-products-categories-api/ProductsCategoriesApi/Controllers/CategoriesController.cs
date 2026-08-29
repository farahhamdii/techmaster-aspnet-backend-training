using Microsoft.AspNetCore.Mvc;
using ProductsCategoriesApi.DTOs;
using ProductsCategoriesApi.Services;

namespace ProductsCategoriesApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var categories = _categoryService.GetAll();

        return Ok(categories);
    }

    [HttpPost]
    public IActionResult Create(CreateCategoryRequest request)
    {
        try
        {
            var category = _categoryService.Create(request);

            return CreatedAtAction(nameof(GetAll),
                new { id = category.CategoryId },category);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }
}