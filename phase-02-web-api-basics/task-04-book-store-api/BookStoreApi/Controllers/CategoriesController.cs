using BookStoreApi.DTOs;
using BookStoreApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApi.Controllers;

[ApiController]
[Route("api/categories")]
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

    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        var category = _categoryService.GetById(id);

        if (category == null)
        {
            return NotFound(new
            {
                message = "Category not found."
            });
        }

        return Ok(category);
    }

    [HttpPost]
    public IActionResult Create(CreateCategoryRequest request)
    {
        var result = _categoryService.Create(request);

        if (!result.Success)
        {
            return Conflict(new
            {
                message = "Category name already exists."
            });
        }

        return CreatedAtAction(
            nameof(GetById),
            new { id = result.Category!.CategoryId },
            result.Category);
    }
}