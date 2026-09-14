using EFCoreModelingDrills.DTOs;
using EFCoreModelingDrills.Entities;

namespace EFCoreModelingDrills.Services;

public interface IStudentService
{
    Task<Student> CreateAsync(Student student);

    Task<Student?> UpdateAsync(int id, Student student);

    Task<List<StudentListItemDto>> GetAllAsync();

    Task<PaginationResult<StudentListItemDto>> GetStudentsAsync(int pageNumber, int pageSize);
}