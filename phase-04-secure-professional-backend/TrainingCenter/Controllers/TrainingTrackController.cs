
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrainingCenter.Common;
using TrainingCenter.DTOs;
using TrainingCenter.Services;

namespace TrainingCenter.Api.Controllers;

[ApiController]
[Route("api/tracks")]
[Authorize]
public class TrainingTrackController : ControllerBase
{
    private readonly ITrainingTrackService _trackService;
    public TrainingTrackController(ITrainingTrackService trackService)
    {
        _trackService = trackService;
    }

    // Admin, Instructor, Student
    [HttpGet]
    [Authorize(Roles = "Admin,Instructor,Student")]
    public async Task<IActionResult> GetAll(string? keyword,string? level,
        string? status,
        int? instructorId)
    {
        var tracks = await _trackService.GetAllAsync(keyword,level,
            status,
            instructorId);

        return Ok(ApiResponse<List<TrackListItemResponse>>.SuccessResponse(tracks,"Tracks retrieved successfully."));
    }

    // Student only
    [HttpGet("/api/tracks/available")]
    [Authorize(Roles = "Student")]
    public async Task<IActionResult> GetAvailableTracks()
    {
        var tracks = await _trackService.GetAvailableAsync();
        return Ok(ApiResponse<List<TrackListItemResponse>>.SuccessResponse(tracks,"Available tracks retrieved successfully."));
    }

    // Admin, Instructor, Student
    [HttpGet("{id:int}")]
    [Authorize(Roles = "Admin,Instructor,Student")]
    public async Task<IActionResult> GetById(int id)
    {
        var track = await _trackService.GetByIdAsync(id);
        if (track == null)
        {
            return NotFound(ApiResponse<object>.ErrorResponse("Track not found."));
        }
        return Ok(ApiResponse<TrackDetailsResponse>.SuccessResponse(track,"Track retrieved successfully."));
    }

    // Admin only
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(CreateTrackRequest request)
    {
        try
        {
            var track = await _trackService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById),
                new { id = track.TrainingTrackId },
                ApiResponse<TrackDetailsResponse>.SuccessResponse(track,"Track created successfully."));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
        }
    }

    // Admin only
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id,UpdateTrackRequest request)
    {
        try
        {
            var track = await _trackService.UpdateAsync(id, request);
            if (track == null)
            {
                return NotFound(ApiResponse<object>.ErrorResponse("Track not found."));
            }

            return Ok(ApiResponse<TrackDetailsResponse>.SuccessResponse( track,"Track updated successfully."));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
        }
    }

    // Admin only
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var deleted = await _trackService.DeleteAsync(id);
            if (!deleted)
            {
                return NotFound(ApiResponse<object>.ErrorResponse("Track not found."));
            }

            return Ok(ApiResponse<object>.SuccessResponse(null, "Track deleted successfully."));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
        }
    }
}
