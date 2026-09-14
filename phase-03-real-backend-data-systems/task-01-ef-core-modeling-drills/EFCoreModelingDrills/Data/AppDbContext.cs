using EFCoreModelingDrills.Entities;
using Microsoft.EntityFrameworkCore;

namespace EFCoreModelingDrills.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Student> Students { get; set; }
    public DbSet<StudentProfile> StudentProfiles { get; set; }
    public DbSet<Instructor> Instructors { get; set; }

    public DbSet<Track> Tracks { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }
    public DbSet<PaymentSummary> PaymentSummaries { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(AppDbContext).Assembly);

        modelBuilder.Entity<Student>().HasData(
            new Student
            {
                Id = 101,
                FullName = "Ahmed Ali",
                Email = "ahmed.ali@techmaster.com",
                CreatedAt = new DateTime(2026, 1, 10),
                IsActive = true
            },
            new Student
            {
                Id = 102,
                FullName = "Mona Hassan",
                Email = "mona.hassan@techmaster.com",
                CreatedAt = new DateTime(2026, 1, 12),
                IsActive = true
            },
            new Student
            {
                Id = 103,
                FullName = "Omar Mohamed",
                Email = "omar.mohamed@techmaster.com",
                CreatedAt = new DateTime(2026, 1, 15),
                IsActive = true
            },
            new Student
            {
                Id = 104,
                FullName = "Sara Mahmoud",
                Email = "sara.mahmoud@techmaster.com",
                CreatedAt = new DateTime(2026, 1, 18),
                IsActive = true
            },
            new Student
            {
                Id = 105,
                FullName = "Youssef Ibrahim",
                Email = "youssef.ibrahim@techmaster.com",
                CreatedAt = new DateTime(2026, 1, 20),
                IsActive = true
            }
        );

        modelBuilder.Entity<Instructor>().HasData(
            new Instructor
            {
                Id = 101,
                FullName = "Ahmed Samir"
            },
            new Instructor
            {
                Id = 102,
                FullName = "Nour Khaled"
            }
        );

        modelBuilder.Entity<Track>().HasData(
            new Track
            {
                Id = 101,
                Name = "ASP.NET Core Backend",
                InstructorId = 101
            },
            new Track
            {
                Id = 102,
                Name = "Database & EF Core",
                InstructorId = 101
            },
            new Track
            {
                Id = 103,
                Name = "Web API Development",
                InstructorId = 102
            }
        );

        modelBuilder.Entity<Enrollment>().HasData(
            new Enrollment
            {
                Id = 101,
                StudentId = 101,
                TrackId = 101,
                Status = "Active",
                EnrollmentDate = new DateTime(2026, 2, 1),
                FinalGrade = null
            },
            new Enrollment
            {
                Id = 102,
                StudentId = 101,
                TrackId = 102,
                Status = "Completed",
                EnrollmentDate = new DateTime(2026, 2, 2),
                FinalGrade = 92.50m
            },
            new Enrollment
            {
                Id = 103,
                StudentId = 102,
                TrackId = 101,
                Status = "Active",
                EnrollmentDate = new DateTime(2026, 2, 3),
                FinalGrade = null
            },
            new Enrollment
            {
                Id = 104,
                StudentId = 103,
                TrackId = 103,
                Status = "Completed",
                EnrollmentDate = new DateTime(2026, 2, 4),
                FinalGrade = 88.00m
            },
            new Enrollment
            {
                Id = 105,
                StudentId = 104,
                TrackId = 102,
                Status = "Active",
                EnrollmentDate = new DateTime(2026, 2, 5),
                FinalGrade = null
            }
        );

        base.OnModelCreating(modelBuilder);
    }
}