using Microsoft.AspNetCore.Mvc;
using ProductsCategoriesApi.DTOs;
using ProductsCategoriesApi.Services;

namespace ProductsCategoriesApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    // GET: /api/products
    [HttpGet]
    public IActionResult GetAll(
        [FromQuery] string? search,
        [FromQuery] int? categoryId,
        [FromQuery] decimal? minPrice,
        [FromQuery] decimal? maxPrice,
        [FromQuery] bool? isAvailable,
        [FromQuery] bool? lowStock)
    {
        var products = _productService.GetAll(
            search,
            categoryId,
            minPrice,
            maxPrice,
            isAvailable,
            lowStock);

        return Ok(products);
    }

    // GET: /api/products/{id}
    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        var product = _productService.GetById(id);

        if (product == null)
        {
            return NotFound(new
            {
                message = $"Product with id {id} was not found."
            });
        }

        return Ok(product);
    }

    // POST: /api/products
    [HttpPost]
    public IActionResult Create(CreateProductRequest request)
    {
        try
        {
            var product = _productService.Create(request);

            return CreatedAtAction(
                nameof(GetById),
                new { id = product.ProductId },
                product);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }

    // PUT: /api/products/{id}
    [HttpPut("{id:int}")]
    public IActionResult Update(
        int id,
        UpdateProductRequest request)
    {
        try
        {
            var product = _productService.Update(id, request);

            if (product == null)
            {
                return NotFound(new
                {
                    message = $"Product with id {id} was not found."
                });
            }

            return Ok(product);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }

    // PATCH: /api/products/{id}/stock
    [HttpPatch("{id:int}/stock")]
    public IActionResult UpdateStock(
        int id,
        UpdateProductStockRequest request)
    {
        var product = _productService.UpdateStock(id, request);

        if (product == null)
        {
            return NotFound(new
            {
                message = $"Product with id {id} was not found."
            });
        }

        return Ok(product);
    }

    // DELETE: /api/products/{id}
    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var deleted = _productService.Delete(id);

        if (!deleted)
        {
            return NotFound(new
            {
                message = $"Product with id {id} was not found."
            });
        }

        return Ok(new
        {
            message = "Product marked as unavailable successfully."
        });
    }

    // GET: /api/products/low-stock
    [HttpGet("low-stock")]
    public IActionResult GetLowStock()
    {
        var products = _productService.GetLowStock();

        return Ok(products);
    }

    // GET: /api/products/reports/stock-value
    [HttpGet("reports/stock-value")]
    public IActionResult GetStockReport()
    {
        var report = _productService.GetStockReport();

        return Ok(report);
    }
}