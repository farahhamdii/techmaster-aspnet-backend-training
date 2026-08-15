using ProductCatalog.Models;

namespace ProductCatalog.Services;

public class ProductQueryService
{
    private readonly List<Product> _products = new();

    public ProductQueryService()
    {
        SeedProducts();
    }

    private void SeedProducts()
    {
        _products.AddRange(new List<Product>
{
    new Product
    {
        ProductId = 1,
        Name = "Laptop Pro 14",
        Category = "Electronics",
        Price = 45000,
        StockQuantity = 5,
        CreatedAt = new DateTime(2026, 1, 10),
        IsAvailable = true,
        SupplierName = "TechSupplier"
    },

    new Product
    {
        ProductId = 2,
        Name = "Wireless Mouse",
        Category = "Electronics",
        Price = 650,
        StockQuantity = 50,
        CreatedAt = new DateTime(2026, 2, 1),
        IsAvailable = true,
        SupplierName = "TechSupplier"
    },

    new Product
    {
        ProductId = 3,
        Name = "Office Chair",
        Category = "Furniture",
        Price = 3500,
        StockQuantity = 10,
        CreatedAt = new DateTime(2025, 12, 15),
        IsAvailable = true,
        SupplierName = "HomeSupplier"
    },

    new Product
    {
        ProductId = 4,
        Name = "Standing Desk",
        Category = "Furniture",
        Price = 8000,
        StockQuantity = 3,
        CreatedAt = new DateTime(2026, 3, 5),
        IsAvailable = true,
        SupplierName = "HomeSupplier"
    },

    new Product
    {
        ProductId = 5,
        Name = "Notebook Pack",
        Category = "Stationery",
        Price = 120,
        StockQuantity = 100,
        CreatedAt = new DateTime(2026, 1, 20),
        IsAvailable = true,
        SupplierName = "PaperSupplier"
    },

    new Product
    {
        ProductId = 6,
        Name = "Pen Set",
        Category = "Stationery",
        Price = 75,
        StockQuantity = 200,
        CreatedAt = new DateTime(2026, 1, 25),
        IsAvailable = true,
        SupplierName = "PaperSupplier"
    },

    new Product
    {
        ProductId = 7,
        Name = "Gaming Keyboard",
        Category = "Electronics",
        Price = 2500,
        StockQuantity = 7,
        CreatedAt = new DateTime(2026, 2, 12),
        IsAvailable = true,
        SupplierName = "TechSupplier"
    },

    new Product
    {
        ProductId = 8,
        Name = "Monitor 27 inch",
        Category = "Electronics",
        Price = 9000,
        StockQuantity = 4,
        CreatedAt = new DateTime(2026, 2, 20),
        IsAvailable = true,
        SupplierName = "TechSupplier"
    },

    new Product
    {
        ProductId = 9,
        Name = "Desk Lamp",
        Category = "Furniture",
        Price = 650,
        StockQuantity = 0,
        CreatedAt = new DateTime(2025, 11, 1),
        IsAvailable = false,
        SupplierName = "HomeSupplier"
    },

    new Product
    {
        ProductId = 10,
        Name = "Backpack",
        Category = "Accessories",
        Price = 1200,
        StockQuantity = 15,
        CreatedAt = new DateTime(2026, 3, 10),
        IsAvailable = true,
        SupplierName = "BagSupplier"
    },

    new Product
    {
        ProductId = 11,
        Name = "USB-C Hub",
        Category = "Electronics",
        Price = 1250,
        StockQuantity = 12,
        CreatedAt = new DateTime(2026, 4, 1),
        IsAvailable = true,
        SupplierName = "TechSupplier"
    },

    new Product
    {
        ProductId = 12,
        Name = "Whiteboard Markers",
        Category = "Stationery",
        Price = 95,
        StockQuantity = 80,
        CreatedAt = new DateTime(2026, 2, 15),
        IsAvailable = true,
        SupplierName = "PaperSupplier"
    },

    new Product
    {
        ProductId = 13,
        Name = "Ergonomic Mouse Pad",
        Category = "Accessories",
        Price = 350,
        StockQuantity = 25,
        CreatedAt = new DateTime(2026, 5, 1),
        IsAvailable = true,
        SupplierName = "BagSupplier"
    },

    new Product
    {
        ProductId = 14,
        Name = "Meeting Table",
        Category = "Furniture",
        Price = 12500,
        StockQuantity = 2,
        CreatedAt = new DateTime(2025, 10, 20),
        IsAvailable = true,
        SupplierName = "HomeSupplier"
    },

    new Product
    {
        ProductId = 15,
        Name = "HD Webcam",
        Category = "Electronics",
        Price = 1800,
        StockQuantity = 6,
        CreatedAt = new DateTime(2026, 4, 17),
        IsAvailable = true,
        SupplierName = "TechSupplier"
    },

    new Product
    {
        ProductId = 16,
        Name = "Printer Paper Box",
        Category = "Stationery",
        Price = 450,
        StockQuantity = 30,
        CreatedAt = new DateTime(2026, 2, 28),
        IsAvailable = true,
        SupplierName = "PaperSupplier"
    },

    new Product
    {
        ProductId = 17,
        Name = "Laptop Stand",
        Category = "Accessories",
        Price = 950,
        StockQuantity = 9,
        CreatedAt = new DateTime(2026, 3, 30),
        IsAvailable = true,
        SupplierName = "BagSupplier"
    },

    new Product
    {
        ProductId = 18,
        Name = "Network Cable 5m",
        Category = "Electronics",
        Price = 150,
        StockQuantity = 60,
        CreatedAt = new DateTime(2026, 1, 5),
        IsAvailable = true,
        SupplierName = "TechSupplier"
    },

    new Product
    {
        ProductId = 19,
        Name = "Storage Cabinet",
        Category = "Furniture",
        Price = 6000,
        StockQuantity = 1,
        CreatedAt = new DateTime(2025, 9, 10),
        IsAvailable = true,
        SupplierName = "HomeSupplier"
    },

    new Product
    {
        ProductId = 20,
        Name = "Sticky Notes",
        Category = "Stationery",
        Price = 60,
        StockQuantity = 0,
        CreatedAt = new DateTime(2026, 5, 10),
        IsAvailable = false,
        SupplierName = "PaperSupplier"
    },

    new Product
    {
        ProductId = 21,
        Name = "Noise Cancelling Headset",
        Category = "Electronics",
        Price = 5200,
        StockQuantity = 4,
        CreatedAt = new DateTime(2026, 3, 22),
        IsAvailable = true,
        SupplierName = "TechSupplier"
    },

    new Product
    {
        ProductId = 22,
        Name = "Desk Organizer",
        Category = "Accessories",
        Price = 300,
        StockQuantity = 40,
        CreatedAt = new DateTime(2026, 6, 1),
        IsAvailable = true,
        SupplierName = "BagSupplier"
    },

    new Product
    {
        ProductId = 23,
        Name = "Projector",
        Category = "Electronics",
        Price = 22000,
        StockQuantity = 2,
        CreatedAt = new DateTime(2026, 4, 28),
        IsAvailable = true,
        SupplierName = "TechSupplier"
    },

    new Product
    {
        ProductId = 24,
        Name = "Office Sofa",
        Category = "Furniture",
        Price = 15500,
        StockQuantity = 1,
        CreatedAt = new DateTime(2025, 8, 18),
        IsAvailable = true,
        SupplierName = "HomeSupplier"
    },

    new Product
    {
        ProductId = 25,
        Name = "Calculator",
        Category = "Stationery",
        Price = 250,
        StockQuantity = 35,
        CreatedAt = new DateTime(2026, 1, 12),
        IsAvailable = true,
        SupplierName = "PaperSupplier"
    }
});

    }
    // Query 01 - Get All Available Products
    public List<Product> GetAvailableProducts() =>
        _products.Where(p => p.IsAvailable).ToList();

    // Query 02 - Filter by Category
    public List<Product> FilterByCategory(string category) =>
        _products.Where(p => p.Category.Equals(category, StringComparison.OrdinalIgnoreCase)).ToList();

    // Query 03 - Filter by Price Range
    public List<Product> FilterByPriceRange(decimal min, decimal max)
    {
        if (min < 0 || max < min) throw new ArgumentException("Invalid price range.");
        return _products.Where(p => p.Price >= min && p.Price <= max).ToList();
    }

    // Query 04 - Search by Product Name
    public List<Product> SearchByName(string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword)) throw new ArgumentException("Keyword cannot be empty.");
        return _products.Where(p => p.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    // Query 05 - Sort by Price Ascending
    public List<Product> SortByPriceAscending() =>
        _products.OrderBy(p => p.Price).ToList();

    // Query 06 - Sort by Price Descending
    public List<Product> SortByPriceDescending() =>
        _products.OrderByDescending(p => p.Price).ToList();

    // Query 07 - Group Products by Category
    public IEnumerable<IGrouping<string, Product>> GroupByCategory() =>
        _products.GroupBy(p => p.Category);

    // Query 08 - Count Products per Category
    public Dictionary<string, int> CountProductsPerCategory() =>
        _products.GroupBy(p => p.Category).ToDictionary(g => g.Key, g => g.Count());

    // Query 09 - Calculate Total Stock Value
    public decimal CalculateTotalStockValue() =>
        _products.Sum(p => p.Price * p.StockQuantity);

    // Query 10 - Stock Value per Category
    public Dictionary<string, decimal> GetStockValuePerCategory() =>
        _products.GroupBy(p => p.Category).ToDictionary(g => g.Key, g => g.Sum(p => p.Price * p.StockQuantity));

    // Query 11 - Top 5 Most Expensive Products
    public List<Product> GetTop5MostExpensiveProducts() =>
        _products.OrderByDescending(p => p.Price).Take(5).ToList();

    // Query 12 - Low Stock Products
    public List<Product> GetLowStockProducts() =>
        _products.Where(p => p.StockQuantity <= 5).ToList();

    // Query 13 - Out of Stock Products
    public List<Product> GetOutOfStockProducts() =>
        _products.Where(p => p.StockQuantity == 0 || !p.IsAvailable).ToList();

    // Query 14 - Product Summary DTO Projection
    public List<ProductSummary> GetProductSummaries() =>
        _products.Select(p => new ProductSummary
        {
            ProductId = p.ProductId,
            Name = p.Name,
            Price = p.Price,
            StockQuantity = p.StockQuantity,
            StockStatus = p.StockQuantity == 0 ? "Out of Stock" : p.StockQuantity <= 5 ? "Low Stock" : "In Stock"
        }).ToList();

    // Query 15 - Supplier Report
    public List<SupplierReport> GetSupplierReport() =>
        _products.GroupBy(p => p.SupplierName).Select(g => new SupplierReport
        {
            SupplierName = g.Key,
            ProductCount = g.Count(),
            StockValue = g.Sum(p => p.Price * p.StockQuantity),
            AveragePrice = g.Average(p => p.Price)
        }).ToList();

    // Query 16 - Recently Added Products
    public List<Product> GetRecentlyAddedProducts() =>
        _products.Where(p => p.CreatedAt >= DateTime.Today.AddDays(-60)).ToList();

    // Query 17 - Category Statistics
    public List<CategoryStats> GetCategoryStatistics() =>
        _products.GroupBy(p => p.Category).Select(g => new CategoryStats
        {
            Category = g.Key,
            Count = g.Count(),
            AveragePrice = g.Average(p => p.Price),
            MaxPrice = g.Max(p => p.Price),
            MinPrice = g.Min(p => p.Price),
            TotalStockValue = g.Sum(p => p.Price * p.StockQuantity)
        }).ToList();

    // Query 18 - Products Above Average Price
    public List<Product> GetProductsAboveAveragePrice()
    {
        if (!_products.Any()) return new List<Product>();
        var average = _products.Average(p => p.Price);
        return _products.Where(p => p.Price > average).ToList();
    }

    // Query 19 - Search + Filter Combined
    public List<Product> SearchAndFilter(string category, decimal minPrice, decimal maxPrice, bool isAvailable)
    {
        if (minPrice < 0 || maxPrice < minPrice) throw new ArgumentException("Invalid price range.");
        return _products.Where(p => p.Category.Equals(category, StringComparison.OrdinalIgnoreCase))
                        .Where(p => p.Price >= minPrice && p.Price <= maxPrice)
                        .Where(p => p.IsAvailable == isAvailable).ToList();
    }

    // Query 20 - Pagination
    public List<Product> GetProductsPage(int pageNumber, int pageSize)
    {
        if (pageNumber <= 0 || pageSize <= 0) throw new ArgumentException("Page number and page size must be greater than zero.");
        return _products.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
    }
}