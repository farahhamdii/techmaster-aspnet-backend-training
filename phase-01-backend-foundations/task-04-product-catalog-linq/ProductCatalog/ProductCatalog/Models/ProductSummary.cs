namespace ProductCatalog.Models;

public class ProductSummary
{
    public int ProductId { get; set; }

    public string Name { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public int StockQuantity { get; set; }

    public string StockStatus { get; set; } = string.Empty;
}