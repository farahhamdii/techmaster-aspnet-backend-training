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

        public async Task<List<StudentListItemResponse>> GetAllAsync()
        {
            return await _context.Students
                .Where(s=>!s.IsDeleted)
                .Select(s=> new StudentListItemResponse
                {
                    StudentId=s.StudentId,
                    FullName=s.FullName,
                    Email=s.Email,
                    PhoneNumber=s.PhoneNumber,
                    IsActive=s.IsActive
                }).ToListAsync();
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
