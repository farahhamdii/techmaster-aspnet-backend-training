namespace ProductCatalog.Models;

public class Product
{
    public int ProductId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public int StockQuantity { get; set; }

    public DateTime CreatedAt { get; set; }

    public bool IsAvailable { get; set; }

    public string SupplierName { get; set; } = string.Empty;

    public double Rating { get; set; }

    public decimal DiscountPercentage { get; set; }
}