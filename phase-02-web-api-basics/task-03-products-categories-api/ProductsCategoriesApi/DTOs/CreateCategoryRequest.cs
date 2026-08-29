using System.ComponentModel.DataAnnotations;

namespace ProductsCategoriesApi.DTOs;

public class CreateCategoryRequest
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    public string Description { get; set; } = string.Empty;
}