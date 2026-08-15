using ProductCatalog.Services;
using ProductCatalog.UI;

namespace ProductCatalog;

internal class Program
{
    static void Main(string[] args)
    {
        var service = new ProductQueryService();
        var menu = new ConsoleMenu(service);
        menu.Start();
    }
}