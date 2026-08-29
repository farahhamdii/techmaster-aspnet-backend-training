using System.ComponentModel.DataAnnotations;

namespace ProductsCategoriesApi.DTOs;

public class CreateProductRequest
{
    [Required]
    [StringLength(150)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [Range(1, int.MaxValue)]
    public int CategoryId { get; set; }

    [Range(0.01, double.MaxValue)]
    public decimal Price { get; set; }

    [Range(0, int.MaxValue)]
    public int StockQuantity { get; set; }

    public bool IsAvailable { get; set; } = true;

    [StringLength(150)]
    public string SupplierName { get; set; } = string.Empty;
}