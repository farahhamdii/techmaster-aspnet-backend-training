using ProductsCategoriesApi.Data;
using ProductsCategoriesApi.DTOs;
using ProductsCategoriesApi.Models;

namespace ProductsCategoriesApi.Services;

public class ProductService : IProductService
{
    private readonly InMemoryStore _store;

    private const int LowStockThreshold = 5;

    public ProductService(InMemoryStore store)
    {
        _store = store;
    }

    public IEnumerable<ProductResponse> GetAll(
        string? search,
        int? categoryId,
        decimal? minPrice,
        decimal? maxPrice,
        bool? isAvailable,
        bool? lowStock)
    {
        var products = _store.Products.AsEnumerable();

        // Search by product name
        if (!string.IsNullOrWhiteSpace(search))
        {
            products = products.Where(p =>
                p.Name.Contains(
                    search.Trim(),
                    StringComparison.OrdinalIgnoreCase));
        }

        // Filter by category
        if (categoryId.HasValue)
        {
            products = products.Where(p =>
                p.CategoryId == categoryId.Value);
        }

        // Filter by minimum price
        if (minPrice.HasValue)
        {
            products = products.Where(p =>
                p.Price >= minPrice.Value);
        }

        // Filter by maximum price
        if (maxPrice.HasValue)
        {
            products = products.Where(p =>
                p.Price <= maxPrice.Value);
        }

        // Filter by availability
        if (isAvailable.HasValue)
        {
            products = products.Where(p =>
                p.IsAvailable == isAvailable.Value);
        }

        // Filter by low stock
        if (lowStock == true)
        {
            products = products.Where(p =>
                p.StockQuantity <= LowStockThreshold);
        }

        return products
            .Select(MapToResponse)
            .ToList();
    }

    public ProductResponse? GetById(int id)
    {
        var product = _store.Products
            .FirstOrDefault(p => p.ProductId == id);

        return product == null
            ? null
            : MapToResponse(product);
    }

    public ProductResponse Create(CreateProductRequest request)
    {
        var category = _store.Categories
            .FirstOrDefault(c =>
                c.CategoryId == request.CategoryId &&
                c.IsActive);

        if (category == null)
        {
            throw new InvalidOperationException(
                "Category does not exist or is inactive.");
        }

        var product = new Product
        {
            ProductId = _store.NextProductId++,
            Name = request.Name.Trim(),
            CategoryId = request.CategoryId,
            Price = request.Price,
            StockQuantity = request.StockQuantity,
            IsAvailable = request.IsAvailable,
            SupplierName = request.SupplierName.Trim(),
            CreatedAt = DateTime.UtcNow
        };

        _store.Products.Add(product);

        return MapToResponse(product);
    }

    public ProductResponse? Update(
        int id,
        UpdateProductRequest request)
    {
        var product = _store.Products
            .FirstOrDefault(p => p.ProductId == id);

        if (product == null)
        {
            return null;
        }

        var category = _store.Categories
            .FirstOrDefault(c =>
                c.CategoryId == request.CategoryId &&
                c.IsActive);

        if (category == null)
        {
            throw new InvalidOperationException(
                "Category does not exist or is inactive.");
        }

        product.Name = request.Name.Trim();
        product.CategoryId = request.CategoryId;
        product.Price = request.Price;
        product.StockQuantity = request.StockQuantity;
        product.IsAvailable = request.IsAvailable;
        product.SupplierName = request.SupplierName.Trim();

        return MapToResponse(product);
    }

    public ProductResponse? UpdateStock(
        int id,
        UpdateProductStockRequest request)
    {
        var product = _store.Products
            .FirstOrDefault(p => p.ProductId == id);

        if (product == null)
        {
            return null;
        }

        product.StockQuantity = request.StockQuantity;

        return MapToResponse(product);
    }

    public bool Delete(int id)
    {
        var product = _store.Products
            .FirstOrDefault(p => p.ProductId == id);

        if (product == null)
        {
            return false;
        }

        product.IsAvailable = false;

        return true;
    }

    public IEnumerable<ProductResponse> GetLowStock()
    {
        return _store.Products
            .Where(p => p.StockQuantity <= LowStockThreshold)
            .Select(MapToResponse)
            .ToList();
    }

    public StockReportResponse GetStockReport()
    {
        var totalStockValue = _store.Products
            .Sum(p => p.Price * p.StockQuantity);

        var stockValueByCategory = _store.Categories
            .Where(c => c.IsActive)
            .Select(c => new CategoryStockValueResponse
            {
                CategoryId = c.CategoryId,
                CategoryName = c.Name,
                StockValue = _store.Products
                    .Where(p => p.CategoryId == c.CategoryId)
                    .Sum(p => p.Price * p.StockQuantity)
            })
            .ToList();

        var lowStockProducts = _store.Products
            .Where(p => p.StockQuantity <= LowStockThreshold)
            .Select(MapToResponse)
            .ToList();

        var outOfStockProducts = _store.Products
            .Where(p => p.StockQuantity == 0)
            .Select(MapToResponse)
            .ToList();

        var productsCountByCategory = _store.Categories
            .Where(c => c.IsActive)
            .Select(c => new CategoryProductCountResponse
            {
                CategoryId = c.CategoryId,
                CategoryName = c.Name,
                ProductCount = _store.Products
                    .Count(p => p.CategoryId == c.CategoryId)
            })
            .ToList();

        return new StockReportResponse
        {
            TotalStockValue = totalStockValue,
            StockValueByCategory = stockValueByCategory,
            LowStockProducts = lowStockProducts,
            OutOfStockProducts = outOfStockProducts,
            ProductsCountByCategory = productsCountByCategory
        };
    }

    private ProductResponse MapToResponse(Product product)
    {
        var category = _store.Categories
            .FirstOrDefault(c => c.CategoryId == product.CategoryId);

        return new ProductResponse
        {
            ProductId = product.ProductId,
            Name = product.Name,
            CategoryId = product.CategoryId,
            CategoryName = category?.Name ?? "Unknown",
            Price = product.Price,
            StockQuantity = product.StockQuantity,
            IsAvailable = product.IsAvailable,
            SupplierName = product.SupplierName,
            CreatedAt = product.CreatedAt
        };
    }
}