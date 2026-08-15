using ProductCatalog.Models;
using ProductCatalog.Services;

namespace ProductCatalog.UI;

public class ConsoleMenu
{
    private readonly ProductQueryService _service;

    public ConsoleMenu(ProductQueryService service)
    {
        _service = service;
    }

    public void Start()
    {
        bool running = true;

        while (running)
        {
            DisplayMenu();

            Console.Write("Choose an option: ");
            string? choice = Console.ReadLine();

            Console.Clear();

            try
            {
                switch (choice)
                {
                    case "1":
                        ViewAvailableProducts();
                        break;

                    case "2":
                        FilterByCategory();
                        break;

                    case "3":
                        FilterByPriceRange();
                        break;

                    case "4":
                        SearchByName();
                        break;

                    case "5":
                        SortByPrice();
                        break;

                    case "6":
                        GroupByCategory();
                        break;

                    case "7":
                        ShowStockValueReports();
                        break;

                    case "8":
                        ShowLowStockProducts();
                        break;

                    case "9":
                        ShowSupplierReport();
                        break;

                    case "10":
                        PaginationDemo();
                        break;

                    case "11":
                        running = false;
                        Console.WriteLine("Goodbye!");
                        break;

                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            if (running)
            {
                Console.WriteLine("\nPress Enter to continue...");
                Console.ReadLine();
                Console.Clear();
            }
        }
    }

    private void DisplayMenu()
    {
        Console.WriteLine("====== Product Catalog LINQ System ======");
        Console.WriteLine("1. View Available Products");
        Console.WriteLine("2. Filter by Category");
        Console.WriteLine("3. Filter by Price Range");
        Console.WriteLine("4. Search by Name");
        Console.WriteLine("5. Sort by Price");
        Console.WriteLine("6. Group by Category");
        Console.WriteLine("7. Stock Value Reports");
        Console.WriteLine("8. Low Stock Products");
        Console.WriteLine("9. Supplier Report");
        Console.WriteLine("10. Pagination Demo");
        Console.WriteLine("11. Exit");
        Console.WriteLine("=========================================");
    }

    private void ViewAvailableProducts()
    {
        var products = _service.GetAvailableProducts();

        Console.WriteLine("=== Available Products ===");
        DisplayProducts(products);
    }

    private void FilterByCategory()
    {
        Console.Write("Enter category: ");
        string category = Console.ReadLine() ?? "";

        var products = _service.FilterByCategory(category);

        Console.WriteLine($"\n=== Products in {category} ===");
        DisplayProducts(products);
    }

    private void FilterByPriceRange()
    {
        Console.Write("Minimum price: ");
        decimal min = decimal.Parse(Console.ReadLine() ?? "");

        Console.Write("Maximum price: ");
        decimal max = decimal.Parse(Console.ReadLine() ?? "");

        var products = _service.FilterByPriceRange(min, max);

        Console.WriteLine("\n=== Products in Price Range ===");
        DisplayProducts(products);
    }

    private void SearchByName()
    {
        Console.Write("Enter product name or keyword: ");
        string keyword = Console.ReadLine() ?? "";

        var products = _service.SearchByName(keyword);

        Console.WriteLine("\n=== Search Results ===");
        DisplayProducts(products);
    }

    private void SortByPrice()
    {
        Console.WriteLine("1. Price Ascending");
        Console.WriteLine("2. Price Descending");

        Console.Write("Choose: ");
        string? choice = Console.ReadLine();

        var products = choice switch
        {
            "1" => _service.SortByPriceAscending(),
            "2" => _service.SortByPriceDescending(),
            _ => new List<Product>()
        };

        Console.WriteLine("\n=== Sorted Products ===");
        DisplayProducts(products);
    }

    private void GroupByCategory()
    {
        var groups = _service.GroupByCategory();

        Console.WriteLine("=== Products Grouped By Category ===");

        foreach (var group in groups)
        {
            Console.WriteLine($"\n--- {group.Key} ---");

            foreach (var product in group)
            {
                Console.WriteLine($"{product.Name} - {product.Price:C}");
            }
        }
    }

    private void ShowStockValueReports()
    {
        Console.WriteLine("=== Stock Value Reports ===");

        Console.WriteLine(
            $"Total Stock Value: {_service.CalculateTotalStockValue():C}");

        Console.WriteLine("\nStock Value By Category:");

        var reports = _service.GetStockValuePerCategory();

        foreach (var report in reports)
        {
            Console.WriteLine($"{report.Key}: {report.Value:C}");
        }
    }

    private void ShowLowStockProducts()
    {
        var products = _service.GetLowStockProducts();

        Console.WriteLine("=== Low Stock Products ===");

        foreach (var product in products)
        {
            Console.WriteLine(
                $"{product.Name} - Stock: {product.StockQuantity}");
        }
    }

    private void ShowSupplierReport()
    {
        var reports = _service.GetSupplierReport();

        Console.WriteLine("=== Supplier Report ===");

        foreach (var report in reports)
        {
            Console.WriteLine(
                $"{report.SupplierName} | " +
                $"Products: {report.ProductCount} | " +
                $"Stock Value: {report.StockValue:C} | " +
                $"Average Price: {report.AveragePrice:C}");
        }
    }

    private void PaginationDemo()
    {
        Console.Write("Page Number: ");
        int pageNumber = int.Parse(Console.ReadLine() ?? "");

        Console.Write("Page Size: ");
        int pageSize = int.Parse(Console.ReadLine() ?? "");

        var products = _service.GetProductsPage(
            pageNumber,
            pageSize);

        Console.WriteLine(
            $"\n=== Page {pageNumber} ===");

        DisplayProducts(products);
    }

    private void DisplayProducts(IEnumerable<Product> products)
    {
        var productList = products.ToList();

        if (!productList.Any())
        {
            Console.WriteLine("No products found.");
            return;
        }

        foreach (var product in productList)
        {
            Console.WriteLine(
                $"{product.ProductId}. " +
                $"{product.Name} | " +
                $"{product.Category} | " +
                $"{product.Price:C} | " +
                $"Stock: {product.StockQuantity} | " +
                $"Available: {product.IsAvailable}");
        }
    }
}