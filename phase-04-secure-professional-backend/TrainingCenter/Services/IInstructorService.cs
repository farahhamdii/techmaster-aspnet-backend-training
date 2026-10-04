
using TrainingCenter.DTOs;
using TrainingCenter.DTOs.Instructor;

namespace TrainingCenter.Services;

public interface IInstructorService
{
    Task<List<InstructorListItemResponse>> GetAllAsync();

    Task<InstructorDetailsResponse?> GetByIdAsync(int id);

    Task<InstructorListItemResponse> CreateAsync(CreateInstructorRequest request);

    Task<InstructorListItemResponse?> UpdateAsync(int id, UpdateInstructorRequest request);

    Task<List<TrainingTrackListItemResponse>> GetTracksAsync(int id);
}