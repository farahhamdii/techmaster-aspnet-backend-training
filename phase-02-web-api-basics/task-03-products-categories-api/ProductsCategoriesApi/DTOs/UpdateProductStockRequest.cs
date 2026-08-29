using System.ComponentModel.DataAnnotations;

namespace ProductsCategoriesApi.DTOs;

public class UpdateProductStockRequest
{
    [Range(0, int.MaxValue, ErrorMessage = "Stock quantity cannot be negative.")]
    public int StockQuantity { get; set; }
}