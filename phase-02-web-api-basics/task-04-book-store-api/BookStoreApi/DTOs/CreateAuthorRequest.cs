using System.ComponentModel.DataAnnotations;

namespace BookStoreApi.DTOs;

public class CreateAuthorRequest
{
    [Required]
    [MaxLength(150)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Country { get; set; } = string.Empty;

    public DateTime? BirthDate { get; set; }
}