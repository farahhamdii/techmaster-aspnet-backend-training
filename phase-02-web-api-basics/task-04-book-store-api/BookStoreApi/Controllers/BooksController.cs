using BookStoreApi.DTOs;
using BookStoreApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApi.Controllers;

[ApiController]
[Route("api/books")]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;

    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    [HttpGet]
    public IActionResult GetAll(
        [FromQuery] string? search,
        [FromQuery] int? categoryId,
        [FromQuery] int? authorId,
        [FromQuery] bool? isAvailable,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        if (pageNumber < 1)
        {
            return BadRequest(new
            {
                message = "pageNumber must be greater than or equal to 1."
            });
        }

        if (pageSize < 1 || pageSize > 100)
        {
            return BadRequest(new
            {
                message = "pageSize must be between 1 and 100."
            });
        }

        var books = _bookService.GetAll(
            search,
            categoryId,
            authorId,
            isAvailable,
            pageNumber,
            pageSize);

        return Ok(books);
    }

    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        var book = _bookService.GetById(id);

        if (book == null)
        {
            return NotFound(new
            {
                message = "Book not found."
            });
        }

        return Ok(book);
    }

    [HttpPost]
    public IActionResult Create(CreateBookRequest request)
    {
        var result = _bookService.Create(request);

        if (!result.Success)
        {
            return result.Error switch
            {
                "AUTHOR_NOT_FOUND" => NotFound(new
                {
                    message = "Author not found."
                }),

                "CATEGORY_NOT_FOUND" => NotFound(new
                {
                    message = "Category not found."
                }),

                "CATEGORY_INACTIVE" => Conflict(new
                {
                    message = "Inactive categories cannot be assigned to new books."
                }),

                "ISBN_EXISTS" => Conflict(new
                {
                    message = "A book with this ISBN already exists."
                }),

                _ => BadRequest(new
                {
                    message = "Unable to create book."
                })
            };
        }

        return CreatedAtAction(
            nameof(GetById),
            new { id = result.Book!.BookId },
            result.Book);
    }

    [HttpPut("{id:int}")]
    public IActionResult Update(
        int id,
        UpdateBookRequest request)
    {
        var result = _bookService.Update(id, request);

        if (!result.Success)
        {
            return result.Error switch
            {
                "BOOK_NOT_FOUND" => NotFound(new
                {
                    message = "Book not found."
                }),

                "AUTHOR_NOT_FOUND" => NotFound(new
                {
                    message = "Author not found."
                }),

                "CATEGORY_NOT_FOUND" => NotFound(new
                {
                    message = "Category not found."
                }),

                "CATEGORY_INACTIVE" => Conflict(new
                {
                    message = "Inactive categories cannot be assigned to a book."
                }),

                "ISBN_EXISTS" => Conflict(new
                {
                    message = "A book with this ISBN already exists."
                }),

                _ => BadRequest(new
                {
                    message = "Unable to update book."
                })
            };
        }

        return Ok(result.Book);
    }

    [HttpGet("reports/summary")]

    public IActionResult GetSummary()
    {
        var summary = _bookService.GetSummary();

        return Ok(summary);
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var success = _bookService.Delete(id);

        if (!success)
        {
            return NotFound(new
            {
                message = "Book not found."
            });
        }

        return NoContent();
    }
}