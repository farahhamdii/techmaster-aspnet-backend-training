using TrainingCenter.DTOs;

namespace TrainingCenter.Services;

public interface ITrainingTrackService
{
    Task<List<TrackListItemResponse>> GetAllAsync( string? keyword,string? level, string? status,int? instructorId);
    Task<TrackDetailsResponse?> GetByIdAsync(int id);
    Task<TrackDetailsResponse> CreateAsync( CreateTrackRequest request);
    Task<TrackDetailsResponse?> UpdateAsync(int id, UpdateTrackRequest request);
    Task<bool> DeleteAsync(int id);
}