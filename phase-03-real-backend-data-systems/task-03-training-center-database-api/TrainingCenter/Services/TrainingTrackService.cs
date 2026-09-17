using Microsoft.EntityFrameworkCore;
using TrainingCenter.Data;
using TrainingCenter.DTOs;
using TrainingCenter.Entities;

namespace TrainingCenter.Services;

public class TrainingTrackService : ITrainingTrackService
{
    private readonly TrainingCenterDbContext _context;

    public TrainingTrackService(TrainingCenterDbContext context)
    {
        _context = context;
    }

    public async Task<List<TrackListItemResponse>> GetAllAsync(
        string? keyword,
        string? level,
        string? status,
        int? instructorId)
    {
        var query = _context.TrainingTracks
            .Where(t => !t.IsDeleted)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            query = query.Where(t =>
                t.Title.Contains(keyword) ||
                t.Code.Contains(keyword));
        }

        if (!string.IsNullOrWhiteSpace(level))
        {
            query = query.Where(t => t.Level == level);
        }

        if (!string.IsNullOrWhiteSpace(status))
        {
            query = query.Where(t => t.Status == status);
        }

        if (instructorId.HasValue)
        {
            query = query.Where(t =>
                t.InstructorId == instructorId.Value);
        }

        return await query
            .Select(t => new TrackListItemResponse
            {
                TrainingTrackId = t.TrainingTrackId,
                Title = t.Title,
                Code = t.Code,
                Level = t.Level,
                Capacity = t.Capacity,
                StartDate = t.StartDate,
                EndDate = t.EndDate,
                Status = t.Status,
                InstructorId = t.InstructorId,
                InstructorName = t.Instructor.FullName
            })
            .ToListAsync();
    }
    public async Task<TrackDetailsResponse?> GetByIdAsync(int id)
    {
        return await _context.TrainingTracks
            .Where(t => t.TrainingTrackId == id && !t.IsDeleted)
            .Select(t => new TrackDetailsResponse
            {
                TrainingTrackId = t.TrainingTrackId,
                Title = t.Title,
                Code = t.Code,
                Description = t.Description,
                Level = t.Level,
                Capacity = t.Capacity,

                EnrolledStudents = t.Enrollments
                    .Count(e => e.Status == "Active"),

                AvailableSeats = t.Capacity -
                    t.Enrollments.Count(e => e.Status == "Active"),

                StartDate = t.StartDate,
                EndDate = t.EndDate,
                Status = t.Status,

                InstructorId = t.InstructorId,
                InstructorName = t.Instructor.FullName
            })
            .FirstOrDefaultAsync();
    }
    public async Task<TrackDetailsResponse> CreateAsync(
    CreateTrackRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
        {
            throw new InvalidOperationException(
                "Track title is required.");
        }
        if (request.Capacity <= 0)
        {
            throw new InvalidOperationException(
                "Capacity must be greater than zero.");
        }

        if (request.EndDate <= request.StartDate)
        {
            throw new InvalidOperationException(
                "End date must be after start date.");
        }

        var instructorExists = await _context.Instructors
            .AnyAsync(i =>
                i.InstructorId == request.InstructorId &&
                i.IsActive);

        if (!instructorExists)
        {
            throw new InvalidOperationException(
                "Instructor not found or inactive.");
        }

        var codeExists = await _context.TrainingTracks
            .AnyAsync(t =>
                t.Code == request.Code &&
                !t.IsDeleted);

        if (codeExists)
        {
            throw new InvalidOperationException(
                "Track code already exists.");
        }

        var track = new TrainingTrack
        {
            Title = request.Title,
            Code = request.Code,
            Description = request.Description,
            Level = request.Level,
            Capacity = request.Capacity,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Status = request.Status,
            InstructorId = request.InstructorId
        };

        _context.TrainingTracks.Add(track);

        await _context.SaveChangesAsync();

        return (await GetByIdAsync(track.TrainingTrackId))!;
    }
    public async Task<TrackDetailsResponse?> UpdateAsync(
    int id,
    UpdateTrackRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
        {
            throw new InvalidOperationException(
                "Track title is required.");
        }
        var track = await _context.TrainingTracks
            .FirstOrDefaultAsync(t =>
                t.TrainingTrackId == id &&
                !t.IsDeleted);

        if (track == null)
        {
            return null;
        }

        if (request.Capacity <= 0)
        {
            throw new InvalidOperationException(
                "Capacity must be greater than zero.");
        }

        if (request.EndDate <= request.StartDate)
        {
            throw new InvalidOperationException(
                "End date must be after start date.");
        }

        var instructorExists = await _context.Instructors
            .AnyAsync(i =>
                i.InstructorId == request.InstructorId &&
                i.IsActive);

        if (!instructorExists)
        {
            throw new InvalidOperationException(
                "Instructor not found or inactive.");
        }

        var codeExists = await _context.TrainingTracks
            .AnyAsync(t =>
                t.Code == request.Code &&
                t.TrainingTrackId != id &&
                !t.IsDeleted);

        if (codeExists)
        {
            throw new InvalidOperationException(
                "Track code already exists.");
        }

        var activeEnrollments = await _context.Enrollments
            .CountAsync(e =>
                e.TrainingTrackId == id &&
                e.Status == "Active");

        if (request.Capacity < activeEnrollments)
        {
            throw new InvalidOperationException(
                "Capacity cannot be less than current active enrollments.");
        }

        track.Title = request.Title;
        track.Code = request.Code;
        track.Description = request.Description;
        track.Level = request.Level;
        track.Capacity = request.Capacity;
        track.StartDate = request.StartDate;
        track.EndDate = request.EndDate;
        track.Status = request.Status;
        track.InstructorId = request.InstructorId;

        await _context.SaveChangesAsync();

        return await GetByIdAsync(id);
    }
    public async Task<bool> DeleteAsync(int id)
    {
        var track = await _context.TrainingTracks
            .FirstOrDefaultAsync(t =>
                t.TrainingTrackId == id &&
                !t.IsDeleted);

        if (track == null)
        {
            return false;
        }

        var hasActiveEnrollments = await _context.Enrollments
            .AnyAsync(e =>
                e.TrainingTrackId == id &&
                e.Status == "Active");

        if (hasActiveEnrollments)
        {
            throw new InvalidOperationException(
                "Cannot delete a track with active enrollments.");
        }

        track.IsDeleted = true;

        await _context.SaveChangesAsync();

        return true;
    }
}