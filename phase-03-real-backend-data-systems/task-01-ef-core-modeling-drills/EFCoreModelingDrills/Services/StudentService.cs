using EFCoreModelingDrills.Data;
using EFCoreModelingDrills.DTOs;
using EFCoreModelingDrills.Entities;
using Microsoft.EntityFrameworkCore;

namespace EFCoreModelingDrills.Services;

public class StudentService : IStudentService
{
    private readonly AppDbContext _context;

    public StudentService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Student> CreateAsync(Student student)
    {
        student.CreatedAt = DateTime.UtcNow;
        student.UpdatedAt = null;

        _context.Students.Add(student);

        await _context.SaveChangesAsync();

        return student;
    }

    public async Task<Student?> UpdateAsync(int id, Student student)
    {
        var existingStudent = await _context.Students
            .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);

        if (existingStudent == null)
        {
            return null;
        }

        existingStudent.FullName = student.FullName;
        existingStudent.Email = student.Email;
        existingStudent.IsActive = student.IsActive;

        existingStudent.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return existingStudent;
    }
    public async Task<List<StudentListItemDto>> GetAllAsync()
    {
        return await _context.Students
            .Where(s => !s.IsDeleted)
            .Select(s => new StudentListItemDto
            {
                Id = s.Id,
                FullName = s.FullName,
                Email = s.Email,
                IsActive = s.IsActive
            })
            .ToListAsync();
    }
    public async Task<PaginationResult<StudentListItemDto>> GetStudentsAsync(
    int pageNumber,
    int pageSize)
    {
        var totalCount = await _context.Students.Where(s => !s.IsDeleted).CountAsync();

        var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

        var skip = (pageNumber - 1) * pageSize;

        var items = await _context.Students
            .Select(s => new StudentListItemDto
            {
                Id = s.Id,
                FullName = s.FullName,
                Email = s.Email,
                IsActive = s.IsActive
            })
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync();

        return new PaginationResult<StudentListItemDto>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalPages = totalPages
        };
    }
}