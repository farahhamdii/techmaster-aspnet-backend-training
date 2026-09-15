using Microsoft.AspNetCore.Mvc;
using TrainingCenter.Common;
using TrainingCenter.DTOs;
using TrainingCenter.Services;

namespace TrainingCenter.Api.Controllers;

[ApiController]
[Route("api/tracks")]
public class TrainingTrackController : ControllerBase
{
    private readonly ITrainingTrackService _trackService;

    public TrainingTrackController(ITrainingTrackService trackService)
    {
        _trackService = trackService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        string? keyword,
        string? level,
        string? status,
        int? instructorId)
    {
        var tracks = await _trackService.GetAllAsync(
            keyword,
            level,
            status,
            instructorId);

        return Ok(new ApiResponse<List<TrackListItemResponse>>
        {
            Success = true,
            Message = "Tracks retrieved successfully.",
            Data = tracks
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var track = await _trackService.GetByIdAsync(id);

        if (track == null)
        {
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = "Track not found."
            });
        }

        return Ok(new ApiResponse<TrackDetailsResponse>
        {
            Success = true,
            Message = "Track retrieved successfully.",
            Data = track
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateTrackRequest request)
    {
        try
        {
            var track = await _trackService.CreateAsync(request);

            return CreatedAtAction(
                nameof(GetById),
                new { id = track.TrainingTrackId },
                new ApiResponse<TrackDetailsResponse>
                {
                    Success = true,
                    Message = "Track created successfully.",
                    Data = track
                });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new ApiResponse<object>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        int id,
        UpdateTrackRequest request)
    {
        try
        {
            var track = await _trackService.UpdateAsync(id, request);

            if (track == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Track not found."
                });
            }

            return Ok(new ApiResponse<TrackDetailsResponse>
            {
                Success = true,
                Message = "Track updated successfully.",
                Data = track
            });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new ApiResponse<object>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var deleted = await _trackService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Track not found."
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Track deleted successfully."
            });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new ApiResponse<object>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }
}