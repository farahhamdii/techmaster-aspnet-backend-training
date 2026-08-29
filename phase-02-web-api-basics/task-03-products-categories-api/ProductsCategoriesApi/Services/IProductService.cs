using ProductsCategoriesApi.DTOs;

namespace ProductsCategoriesApi.Services;

public interface IProductService
{
    IEnumerable<ProductResponse> GetAll(
        string? search,
        int? categoryId,
        decimal? minPrice,
        decimal? maxPrice,
        bool? isAvailable,
        bool? lowStock);

    ProductResponse? GetById(int id);

    ProductResponse Create(CreateProductRequest request);

    ProductResponse? Update(int id, UpdateProductRequest request);

    ProductResponse? UpdateStock(
        int id,
        UpdateProductStockRequest request);

    bool Delete(int id);

    IEnumerable<ProductResponse> GetLowStock();

    StockReportResponse GetStockReport();
}