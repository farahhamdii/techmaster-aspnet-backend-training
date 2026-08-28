using StudentManagementApi.DTOs;
using StudentManagementApi.Models;

namespace StudentManagementApi.Services;

public class StudentService : IStudentService
{
    private static readonly List<Student> _students = new();
    private static int _nextId = 1;
    private static readonly object _lock = new();

    public StudentService()
    {
        lock (_lock)
        {
            if (!_students.Any())
            {
                SeedInitialData();
            }
        }
    }

    private void SeedInitialData()
    {
        _students.AddRange(new List<Student>
        {
            new Student
            {
                StudentId = _nextId++,
                FullName = "Ahmed Hassan",
                Email = "ahmed.hassan@example.com",
                PhoneNumber = "+201012345678",
                TrackName = ".NET Backend",
                EnrollmentDate = DateTime.UtcNow.AddMonths(-3),
                IsActive = true,
                GitHubProfileUrl = "https://github.com/ahmedhassan",
                LinkedInProfileUrl = "https://linkedin.com/in/ahmedhassan"
            },
            new Student
            {
                StudentId = _nextId++,
                FullName = "Sara Ali",
                Email = "sara.ali@example.com",
                PhoneNumber = "+201123456789",
                TrackName = "Frontend React",
                EnrollmentDate = DateTime.UtcNow.AddMonths(-2),
                IsActive = true
            },
            new Student
            {
                StudentId = _nextId++,
                FullName = "Mohamed Mahmoud",
                Email = "m.mahmoud@example.com",
                PhoneNumber = "+201234567890",
                TrackName = ".NET Backend",
                EnrollmentDate = DateTime.UtcNow.AddMonths(-1),
                IsActive = false
            }
        });
    }

    public PagedResultResponse<StudentResponse> GetAll(string? search, string? trackName, bool? isActive, int pageNumber, int pageSize)
    {
        var query = _students.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim().ToLower();
            query = query.Where(s => s.FullName.ToLower().Contains(term) || s.Email.ToLower().Contains(term));
        }

        if (!string.IsNullOrWhiteSpace(trackName))
        {
            query = query.Where(s => s.TrackName.Equals(trackName.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        if (isActive.HasValue)
        {
            query = query.Where(s => s.IsActive == isActive.Value);
        }

        int totalCount = query.Count();
        var items = query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(MapToResponse)
            .ToList();

        return new PagedResultResponse<StudentResponse>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }

    public StudentResponse? GetById(int id)
    {
        var student = _students.FirstOrDefault(s => s.StudentId == id);
        return student == null ? null : MapToResponse(student);
    }

    public IEnumerable<StudentResponse> GetByTrack(string trackName)
    {
        return _students
            .Where(s => s.TrackName.Equals(trackName.Trim(), StringComparison.OrdinalIgnoreCase))
            .Select(MapToResponse)
            .ToList();
    }

    public StudentResponse Create(CreateStudentRequest request)
    {
        lock (_lock)
        {
            var student = new Student
            {
                StudentId = _nextId++,
                FullName = request.FullName.Trim(),
                Email = request.Email.Trim().ToLower(),
                PhoneNumber = request.PhoneNumber.Trim(),
                TrackName = request.TrackName.Trim(),
                EnrollmentDate = DateTime.UtcNow,
                IsActive = true,
                GitHubProfileUrl = request.GitHubProfileUrl?.Trim(),
                LinkedInProfileUrl = request.LinkedInProfileUrl?.Trim()
            };

            _students.Add(student);
            return MapToResponse(student);
        }
    }

    public StudentResponse? Update(int id, UpdateStudentRequest request)
    {
        lock (_lock)
        {
            var student = _students.FirstOrDefault(s => s.StudentId == id);
            if (student == null) return null;

            student.FullName = request.FullName.Trim();
            student.Email = request.Email.Trim().ToLower();
            student.PhoneNumber = request.PhoneNumber.Trim();
            student.TrackName = request.TrackName.Trim();
            student.GitHubProfileUrl = request.GitHubProfileUrl?.Trim();
            student.LinkedInProfileUrl = request.LinkedInProfileUrl?.Trim();

            return MapToResponse(student);
        }
    }

    public bool UpdateStatus(int id, bool isActive)
    {
        lock (_lock)
        {
            var student = _students.FirstOrDefault(s => s.StudentId == id);
            if (student == null) return false;

            student.IsActive = isActive;
            return true;
        }
    }

    public bool Delete(int id)
    {
        lock (_lock)
        {
            var student = _students.FirstOrDefault(s => s.StudentId == id);
            if (student == null) return false;

            student.IsActive = false;
            return true;
        }
    }

    public StudentStatsResponse GetStats()
    {
        return new StudentStatsResponse
        {
            TotalStudents = _students.Count,
            ActiveStudents = _students.Count(s => s.IsActive),
            InactiveStudents = _students.Count(s => !s.IsActive),
            CountByTrack = _students
                .GroupBy(s => s.TrackName, StringComparer.OrdinalIgnoreCase)
                .ToDictionary(g => g.Key, g => g.Count())
        };
    }

    public bool IsEmailExists(string email, int? excludeId = null)
    {
        var normalizedEmail = email.Trim().ToLower();
        return _students.Any(s => s.Email.Equals(normalizedEmail, StringComparison.OrdinalIgnoreCase) && (!excludeId.HasValue || s.StudentId != excludeId.Value));
    }

    private static StudentResponse MapToResponse(Student student) => new()
    {
        StudentId = student.StudentId,
        FullName = student.FullName,
        Email = student.Email,
        PhoneNumber = student.PhoneNumber,
        TrackName = student.TrackName,
        EnrollmentDate = student.EnrollmentDate,
        IsActive = student.IsActive,
        GitHubProfileUrl = student.GitHubProfileUrl,
        LinkedInProfileUrl = student.LinkedInProfileUrl
    };
}