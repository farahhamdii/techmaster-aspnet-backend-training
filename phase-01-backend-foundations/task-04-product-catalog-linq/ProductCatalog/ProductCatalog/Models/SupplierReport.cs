namespace ProductCatalog.Models;

public class SupplierReport
{
    public string SupplierName { get; set; } = string.Empty;

    public int ProductCount { get; set; }

    public decimal StockValue { get; set; }

    public decimal AveragePrice { get; set; }
}