using RefactoredApi.DTOs;
using RefactoredApi.Models;

namespace RefactoredApi.Services;

    public class ProductService : IProductService
{
    private readonly List<Product> _products = new();

    private int _nextId = 0;

    public ProductResponse CreateProduct(CreateProductRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            throw new ArgumentException("Product name is required.");
        }

        if (request.Price < 0)
        {
            throw new ArgumentException("Product price cannot be negative.");
        }

        if (request.Stock < 0)
        {
            throw new ArgumentException("Product stock cannot be negative.");
        }

        var product = new Product
        {
            id = ++_nextId,
            Name = request.Name,
            Price = request.Price,
            Stock = request.Stock,
        };
        _products.Add(product);
        return new ProductResponse
        {
            Id = product.id,
            Name = product.Name,
            Price = product.Price,
            Stock = product.Stock,
        };
    }

    public IEnumerable<ProductResponse> GetAll() 
    {
        return _products.Select(product => new ProductResponse
        {
            Id = product.id,
            Name = product.Name,
            Price = product.Price,
            Stock = product.Stock,
        });
    }

    public ProductResponse? GetById(int id)
    {
        var product =_products.FirstOrDefault(p=>p.id==id);
        if (product == null) return null;
        return new ProductResponse
        {
            Id = product.id,
            Name = product.Name,
            Price = product.Price,
            Stock = product.Stock,
        };
    }
    
}
