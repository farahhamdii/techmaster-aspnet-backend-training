namespace ProductsCategoriesApi.DTOs;

public class ProductResponse
{
    public int ProductId { get; set; }

    public string Name { get; set; } = string.Empty;

    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public int StockQuantity { get; set; }

    public bool IsAvailable { get; set; }

    public string SupplierName { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}