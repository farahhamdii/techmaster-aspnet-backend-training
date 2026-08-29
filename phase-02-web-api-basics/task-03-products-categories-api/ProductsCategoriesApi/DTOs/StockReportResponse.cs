namespace ProductsCategoriesApi.DTOs;

public class StockReportResponse
{
    public decimal TotalStockValue { get; set; }

    public List<CategoryStockValueResponse> StockValueByCategory { get; set; }
        = new();

    public List<ProductResponse> LowStockProducts { get; set; }
        = new();

    public List<ProductResponse> OutOfStockProducts { get; set; }
        = new();

    public List<CategoryProductCountResponse> ProductsCountByCategory { get; set; }
        = new();
}

public class CategoryStockValueResponse
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = string.Empty;

    public decimal StockValue { get; set; }
}

public class CategoryProductCountResponse
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = string.Empty;

    public int ProductCount { get; set; }
}