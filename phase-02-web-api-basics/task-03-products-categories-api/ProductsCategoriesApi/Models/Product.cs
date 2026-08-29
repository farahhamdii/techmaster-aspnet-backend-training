namespace ProductsCategoriesApi.Models;

public class Product
{
    public int ProductId { get; set; }

    public string Name { get; set; } = string.Empty;

    public int CategoryId { get; set; }

    public decimal Price { get; set; }

    public int StockQuantity { get; set; }

    public bool IsAvailable { get; set; }

    public string SupplierName { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public Category? Category { get; set; }
}