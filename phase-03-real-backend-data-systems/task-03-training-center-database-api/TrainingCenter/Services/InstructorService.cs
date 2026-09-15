using Microsoft.EntityFrameworkCore;
using TrainingCenter.Data;
using TrainingCenter.DTOs;
using TrainingCenter.DTOs.Instructor;
using TrainingCenter.Entities;
using TrainingCenter.Services;

namespace TrainingCenter.Services
{
    public class InstructorService : IInstructorService
    {
        private readonly TrainingCenterDbContext _context;
        public InstructorService(TrainingCenterDbContext context)

        {
            _context = context; 
            
        }

        public async Task<List<InstructorListItemResponse>> GetAllAsync()
        {
            return await _context.Instructors
                .Select(i => new InstructorListItemResponse
                {
                    InstructorId = i.InstructorId,
                    FullName = i.FullName,
                    Email = i.Email,
                    Specialization = i.Specialization,
                    IsActive = i.IsActive,
                }).ToListAsync();
        }

        public async Task<InstructorDetailsResponse?>GetByIdAsync(int id )
        {
            return await _context.Instructors
                .Where(i => i.InstructorId == id)
                .Select(i => new InstructorDetailsResponse
                {
                    InstructorId = i.InstructorId,
                    FullName = i.FullName,
                    Email = i.Email,
                    Specialization = i.Specialization,
                    Bio = i.Bio,
                    IsActive = i.IsActive,
                    TotalTracks = i.TrainingTracks.Count(t => !t.IsDeleted)
                })
                .FirstOrDefaultAsync();
        }

        public async Task<InstructorListItemResponse>CreateAsync(CreateInstructorRequest request)
        {
            var emailExists =await _context.Instructors
                .AnyAsync(i=>i.Email== request.Email);
            if (emailExists) 
            {
                throw new InvalidOperationException("email already exists");
            }
            var instructor = new Instructor
            {
                FullName = request.FullName,
                Email = request.Email,
                Specialization = request.Specialization,
                Bio = request.Bio

            };
            _context.Add(instructor);
            await _context.SaveChangesAsync();
            return new InstructorListItemResponse
            {
                InstructorId = instructor.InstructorId,
                FullName = instructor.FullName,
                Email = instructor.Email,
                Specialization = instructor.Specialization,
                IsActive = instructor.IsActive,
            };

        }
        public async Task<InstructorListItemResponse?> UpdateAsync(
        int id,
        UpdateInstructorRequest request)
        {
            var instructor = await _context.Instructors
                .FirstOrDefaultAsync(i => i.InstructorId == id);

            if (instructor == null)
            {
                return null;
            }

            var emailExists = await _context.Instructors
                .AnyAsync(i =>
                    i.Email == request.Email &&
                    i.InstructorId != id);

            if (emailExists)
            {
                throw new InvalidOperationException("Email already exists.");
            }

            instructor.FullName = request.FullName;
            instructor.Email = request.Email;
            instructor.Specialization = request.Specialization;
            instructor.Bio = request.Bio;
            instructor.IsActive = request.IsActive;

            await _context.SaveChangesAsync();

            return new InstructorListItemResponse
            {
                InstructorId = instructor.InstructorId,
                FullName = instructor.FullName,
                Email = instructor.Email,
                Specialization = instructor.Specialization,
                IsActive = instructor.IsActive
            };
        }
        public async Task<List<TrainingTrackListItemResponse>> GetTracksAsync(int id)
        {
            return await _context.TrainingTracks
                .Where(t => t.InstructorId == id && !t.IsDeleted)
                .Select(t => new TrainingTrackListItemResponse
                {
                    TrainingTrackId = t.TrainingTrackId,
                    Title = t.Title,
                    Code = t.Code,
                    Level = t.Level,
                    Capacity = t.Capacity,
                    StartDate = t.StartDate,
                    EndDate = t.EndDate,
                    Status = t.Status
                })
                .ToListAsync();
        }

    }
}
