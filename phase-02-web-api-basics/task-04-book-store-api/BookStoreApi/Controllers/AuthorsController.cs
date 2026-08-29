using BookStoreApi.DTOs;
using BookStoreApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApi.Controllers;

[ApiController]
[Route("api/authors")]
public class AuthorsController : ControllerBase
{
    private readonly IAuthorService _authorService;

    public AuthorsController(IAuthorService authorService)
    {
        _authorService = authorService;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var authors = _authorService.GetAll();

        return Ok(authors);
    }

    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        var author = _authorService.GetById(id);

        if (author == null)
        {
            return NotFound(new
            {
                message = "Author not found."
            });
        }

        return Ok(author);
    }

    [HttpPost]
    public IActionResult Create(CreateAuthorRequest request)
    {
        var author = _authorService.Create(request);

        return CreatedAtAction(nameof(GetById), new { id = author.AuthorId },author);
    }
}