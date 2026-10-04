
using TrainingCenter.DTOs;

namespace TrainingCenter.Services;

public interface IStudentService
{
    Task<List<StudentListItemResponse>> GetAllAsync(
        string? search = null,
        bool? isActive = null);
    Task<PagedResultDto<StudentListItemResponse>> GetPagedStudentsAsync(
    int pageNumber,
    int pageSize);
    Task<StudentDetailsResponse?> GetByIdAsync(int id);

    Task<StudentListItemResponse> CreateAsync(CreateStudentRequest request);

    Task<StudentListItemResponse?> UpdateAsync(int id, UpdateStudentRequest request);
    Task<bool> DeleteAsync(int id);
}