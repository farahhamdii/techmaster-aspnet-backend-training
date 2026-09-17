using Microsoft.EntityFrameworkCore;
using TrainingCenter.Data;
using TrainingCenter.DTOs;
using TrainingCenter.Entities;

namespace TrainingCenter.Services
{
    public class StudentService:IStudentService
    {
        private readonly TrainingCenterDbContext _context;
        public StudentService(TrainingCenterDbContext context)
        {
            _context = context;
            
        }

        public async Task<List<StudentListItemResponse>> GetAllAsync(
      string? search = null,
      bool? isActive = null)
        {
            var query = _context.Students
                .Where(s => !s.IsDeleted);

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(s =>
                    s.FullName.Contains(search) ||
                    s.Email.Contains(search) ||
                    s.PhoneNumber.Contains(search));
            }

            if (isActive.HasValue)
            {
                query = query.Where(s => s.IsActive == isActive.Value);
            }

            return await query
                .Select(s => new StudentListItemResponse
                {
                    StudentId = s.StudentId,
                    FullName = s.FullName,
                    Email = s.Email,
                    PhoneNumber = s.PhoneNumber,
                    IsActive = s.IsActive
                })
                .ToListAsync();
        }
        public async Task<PagedResultDto<StudentListItemResponse>> GetPagedStudentsAsync(
    int pageNumber,
    int pageSize)
        {
            if (pageNumber <= 0)
                throw new ArgumentException("Page number must be greater than 0.");

            if (pageSize <= 0 || pageSize > 100)
                throw new ArgumentException("Page size must be between 1 and 100.");

            var query = _context.Students.Where(s => !s.IsDeleted)
                .AsNoTracking();

            var totalCount = await query.CountAsync();

            var totalPages = (int)Math.Ceiling(
                totalCount / (double)pageSize);

            var students = await query
                .OrderBy(s => s.StudentId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
      .Select(s => new StudentListItemResponse
      {
          StudentId = s.StudentId,
          FullName = s.FullName,
          Email = s.Email,
          PhoneNumber = s.PhoneNumber,
          IsActive = s.IsActive
      })
                .ToListAsync();

            return new PagedResultDto<StudentListItemResponse>
            {
                TotalCount = totalCount,
                TotalPages = totalPages,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Items = students
            };
        }
        public async Task<StudentDetailsResponse?> GetByIdAsync(int id)
        {
            return await _context.Students
                .Where(s => s.StudentId == id && !s.IsDeleted)
                .Select(s => new StudentDetailsResponse
                {
                    StudentId = s.StudentId,
                    FullName = s.FullName,
                    Email = s.Email,
                    PhoneNumber = s.PhoneNumber,
                    IsActive = s.IsActive,
                    TotalEnrollments = s.Enrollments.Count,
                    ActiveEnrollments = s.Enrollments.Count(e => e.Status == "Active")
                })
                .FirstOrDefaultAsync();
        }

        public async Task<StudentListItemResponse> CreateAsync(CreateStudentRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.FullName))
            {
                throw new InvalidOperationException("Full name is required.");
            }
            var emailExists = await _context.Students.AnyAsync(s => s.Email == request.Email && !s.IsDeleted);
            if (emailExists)
            {
                throw new InvalidOperationException("Email already exists.");
            }
            var student = new Student
            {
                FullName = request.FullName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber
            };
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            return new StudentListItemResponse
            {
                StudentId = student.StudentId,
                FullName = student.FullName,
                Email = student.Email,
                PhoneNumber = student.PhoneNumber,
                IsActive = student.IsActive
            };
        }

        public async Task<StudentListItemResponse?> UpdateAsync(int id,UpdateStudentRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.FullName))
            {
                throw new InvalidOperationException("Full name is required.");
            }
            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.StudentId == id && !s.IsDeleted);

            if (student == null)
            {
                return null;
            }

            var emailExists = await _context.Students
                .AnyAsync(s =>
                    s.Email == request.Email &&
                    s.StudentId != id &&
                    !s.IsDeleted);

            if (emailExists)
            {
                throw new InvalidOperationException("Email already exists.");
            }

            student.FullName = request.FullName;
            student.Email = request.Email;
            student.PhoneNumber = request.PhoneNumber;
            student.IsActive = request.IsActive;
            student.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new StudentListItemResponse
            {
                StudentId = student.StudentId,
                FullName = student.FullName,
                Email = student.Email,
                PhoneNumber = student.PhoneNumber,
                IsActive = student.IsActive
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.StudentId == id && !s.IsDeleted);

            if (student == null)
            {
                return false;
            }
            student.IsDeleted = true;
            student.IsActive = false;
            student.DeletedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return true;
        }



    }
}
