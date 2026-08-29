namespace ProductsCategoriesApi.Models;

public class Category
{
    public int CategoryId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public List<Product> Products { get; set; } = new();
}