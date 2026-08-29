using ProductsCategoriesApi.Models;

namespace ProductsCategoriesApi.Data;

public class InMemoryStore
{
    public List<Category> Categories { get; } = new();

    public List<Product> Products { get; } = new();

    public int NextCategoryId { get; set; } = 1;

    public int NextProductId { get; set; } = 1;
    public InMemoryStore()
    {
        SeedCategories();
        SeedProducts();
    }

    private void SeedCategories()
    {
        Categories.AddRange(new[]
        {
            new Category
            {
                CategoryId = 1,
                Name = "Electronics",
                Description = "Electronic devices and accessories",
                IsActive = true,
                CreatedAt = DateTime.UtcNow.AddDays(-10)
            },

            new Category
            {
                CategoryId = 2,
                Name = "Furniture",
                Description = "Office and home furniture",
                IsActive = true,
                CreatedAt = DateTime.UtcNow.AddDays(-9)
            },

            new Category
            {
                CategoryId = 3,
                Name = "Stationery",
                Description = "Office and school stationery",
                IsActive = true,
                CreatedAt = DateTime.UtcNow.AddDays(-8)
            },

            new Category
            {
                CategoryId = 4,
                Name = "Accessories",
                Description = "Bags and computer accessories",
                IsActive = true,
                CreatedAt = DateTime.UtcNow.AddDays(-7)
            }
        });

        NextCategoryId = 5;
    }

    private void SeedProducts()
    {
        Products.AddRange(new[]
        {
            // Electronics
            new Product
            {
                ProductId = 1,
                Name = "Laptop",
                CategoryId = 1,
                Price = 45000,
                StockQuantity = 5,
                IsAvailable = true,
                SupplierName = "Tech Supplier",
                CreatedAt = DateTime.UtcNow.AddDays(-6)
            },

            new Product
            {
                ProductId = 2,
                Name = "Mouse",
                CategoryId = 1,
                Price = 750,
                StockQuantity = 20,
                IsAvailable = true,
                SupplierName = "Tech Supplier",
                CreatedAt = DateTime.UtcNow.AddDays(-6)
            },

            new Product
            {
                ProductId = 3,
                Name = "Keyboard",
                CategoryId = 1,
                Price = 1500,
                StockQuantity = 15,
                IsAvailable = true,
                SupplierName = "Tech Supplier",
                CreatedAt = DateTime.UtcNow.AddDays(-5)
            },

            new Product
            {
                ProductId = 4,
                Name = "Monitor",
                CategoryId = 1,
                Price = 8500,
                StockQuantity = 3,
                IsAvailable = true,
                SupplierName = "Display Store",
                CreatedAt = DateTime.UtcNow.AddDays(-5)
            },

            new Product
            {
                ProductId = 5,
                Name = "USB-C Hub",
                CategoryId = 1,
                Price = 1200,
                StockQuantity = 0,
                IsAvailable = false,
                SupplierName = "Tech Supplier",
                CreatedAt = DateTime.UtcNow.AddDays(-4)
            },

            // Furniture
            new Product
            {
                ProductId = 6,
                Name = "Office Chair",
                CategoryId = 2,
                Price = 6500,
                StockQuantity = 7,
                IsAvailable = true,
                SupplierName = "Office Furniture",
                CreatedAt = DateTime.UtcNow.AddDays(-6)
            },

            new Product
            {
                ProductId = 7,
                Name = "Desk",
                CategoryId = 2,
                Price = 9000,
                StockQuantity = 4,
                IsAvailable = true,
                SupplierName = "Office Furniture",
                CreatedAt = DateTime.UtcNow.AddDays(-5)
            },

            new Product
            {
                ProductId = 8,
                Name = "Desk Lamp",
                CategoryId = 2,
                Price = 950,
                StockQuantity = 2,
                IsAvailable = true,
                SupplierName = "Office Furniture",
                CreatedAt = DateTime.UtcNow.AddDays(-4)
            },

            // Stationery
            new Product
            {
                ProductId = 9,
                Name = "Notebook",
                CategoryId = 3,
                Price = 120,
                StockQuantity = 50,
                IsAvailable = true,
                SupplierName = "Stationery Supplier",
                CreatedAt = DateTime.UtcNow.AddDays(-6)
            },

            new Product
            {
                ProductId = 10,
                Name = "Pen Set",
                CategoryId = 3,
                Price = 200,
                StockQuantity = 30,
                IsAvailable = true,
                SupplierName = "Stationery Supplier",
                CreatedAt = DateTime.UtcNow.AddDays(-5)
            },

            new Product
            {
                ProductId = 11,
                Name = "Marker",
                CategoryId = 3,
                Price = 100,
                StockQuantity = 25,
                IsAvailable = true,
                SupplierName = "Stationery Supplier",
                CreatedAt = DateTime.UtcNow.AddDays(-4)
            },

            new Product
            {
                ProductId = 12,
                Name = "Paper Pack",
                CategoryId = 3,
                Price = 300,
                StockQuantity = 10,
                IsAvailable = true,
                SupplierName = "Stationery Supplier",
                CreatedAt = DateTime.UtcNow.AddDays(-3)
            },

            // Accessories
            new Product
            {
                ProductId = 13,
                Name = "Backpack",
                CategoryId = 4,
                Price = 1800,
                StockQuantity = 8,
                IsAvailable = true,
                SupplierName = "Accessory Supplier",
                CreatedAt = DateTime.UtcNow.AddDays(-6)
            },

            new Product
            {
                ProductId = 14,
                Name = "Mouse Pad",
                CategoryId = 4,
                Price = 350,
                StockQuantity = 1,
                IsAvailable = true,
                SupplierName = "Accessory Supplier",
                CreatedAt = DateTime.UtcNow.AddDays(-4)
            },

            new Product
            {
                ProductId = 15,
                Name = "Laptop Sleeve",
                CategoryId = 4,
                Price = 900,
                StockQuantity = 6,
                IsAvailable = true,
                SupplierName = "Accessory Supplier",
                CreatedAt = DateTime.UtcNow.AddDays(-3)
            }
        });

        NextProductId = 16;
    }
}