using System.ComponentModel.DataAnnotations;

namespace BookStoreApi.DTOs;

public class UpdateBookRequest
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string ISBN { get; set; } = string.Empty;

    [Range(1, 2100)]
    public int PublishedYear { get; set; }

    [Range(0.01, double.MaxValue)]
    public decimal Price { get; set; }

    [Range(0, int.MaxValue)]
    public int StockQuantity { get; set; }

    [Required]
    public int AuthorId { get; set; }

    [Required]
    public int CategoryId { get; set; }

    public bool IsAvailable { get; set; }
}