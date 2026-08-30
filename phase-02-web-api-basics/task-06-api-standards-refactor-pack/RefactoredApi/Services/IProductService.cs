using RefactoredApi.DTOs;

namespace RefactoredApi.Services
{
    public interface IProductService
    {
        ProductResponse CreateProduct(CreateProductRequest request);
        IEnumerable<ProductResponse> GetAll();
        ProductResponse? GetById(int id);
    }
}
