using System.ComponentModel.DataAnnotations;

namespace StudentManagementApi.DTOs;

public class UpdateStudentStatusRequest
{
    [Required]
    public bool IsActive { get; set; }
}
