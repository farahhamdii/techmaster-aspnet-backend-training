using StudentManagementApi.DTOs;

namespace StudentManagementApi.Services;

public interface IStudentService
{
    PagedResultResponse<StudentResponse> GetAll(string? search, string? trackName, bool? isActive, int pageNumber, int pageSize);
    StudentResponse? GetById(int id);
    IEnumerable<StudentResponse> GetByTrack(string trackName);
    StudentResponse Create(CreateStudentRequest request);
    StudentResponse? Update(int id, UpdateStudentRequest request);
    bool UpdateStatus(int id, bool isActive);
    bool Delete(int id);
    StudentStatsResponse GetStats();
    bool IsEmailExists(string email, int? excludeId = null);
}