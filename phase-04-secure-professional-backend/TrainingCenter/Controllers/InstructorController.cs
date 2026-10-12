
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TrainingCenter.Common;
using TrainingCenter.DTOs;
using TrainingCenter.DTOs.Instructor;
using TrainingCenter.Services;

namespace TrainingCenter.Api.Controllers;

[ApiController]
[Route("api/instructors")]
[Authorize]
public class InstructorController : ControllerBase
{
    private readonly IInstructorService _instructorService;
    public InstructorController(IInstructorService instructorService)
    {
        _instructorService = instructorService;
    }

    // Admin only
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll()
    {
        var instructors = await _instructorService.GetAllAsync();
        return Ok(ApiResponse<List<InstructorListItemResponse>>.SuccessResponse(
                instructors,"Instructors retrieved successfully."));
    }

    // Admin → any instructor
    // Instructor → own profile only
    [HttpGet("{id:int}")]
    [Authorize(Roles = "Admin,Instructor")]
    public async Task<IActionResult> GetById(int id)
    {
        if (User.IsInRole("Instructor"))
        {
            var instructorIdClaim = User.FindFirstValue("InstructorId");

            if (!int.TryParse(instructorIdClaim, out var currentInstructorId))
            {
                return Unauthorized();
            }

            if (currentInstructorId != id)
            {
                return Forbid();
            }
        }

        var instructor = await _instructorService.GetByIdAsync(id);

        if (instructor == null)
        {
            return NotFound(ApiResponse<object>.ErrorResponse( "Instructor not found."));
        }
        return Ok(ApiResponse<InstructorDetailsResponse>.SuccessResponse(instructor,
                "Instructor retrieved successfully."));
    }

    // Admin only
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(CreateInstructorRequest request)
    {
        try
        {
            var instructor = await _instructorService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById),
                new { id = instructor.InstructorId },
                ApiResponse<InstructorListItemResponse>.SuccessResponse( instructor,"Instructor created successfully."));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest( ApiResponse<object>.ErrorResponse(ex.Message));
        }
    }

    // Admin only
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update( int id,UpdateInstructorRequest request)
    {
        try
        {
            var instructor = await _instructorService.UpdateAsync(id, request);
            if (instructor == null)
            {
                return NotFound(ApiResponse<object>.ErrorResponse( "Instructor not found."));
            }
            return Ok(ApiResponse<InstructorListItemResponse>.SuccessResponse(
                    instructor, "Instructor updated successfully."));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest( ApiResponse<object>.ErrorResponse(ex.Message));
        }
    }

    // Admin → any instructor's tracks
    // Instructor → own tracks only
    [HttpGet("{id:int}/tracks")]
    [Authorize(Roles = "Admin,Instructor")]
    public async Task<IActionResult> GetTracks(int id)
    {
        if (User.IsInRole("Instructor"))
        {
            var instructorIdClaim = User.FindFirstValue("InstructorId");
            if (!int.TryParse(instructorIdClaim, out var currentInstructorId))
            {
                return Unauthorized();
            }
            if (currentInstructorId != id)
            {
                return Forbid();
            }
        }

        var tracks = await _instructorService.GetTracksAsync(id);
        return Ok( ApiResponse<List<TrainingTrackListItemResponse>>.SuccessResponse(
                tracks, "Instructor tracks retrieved successfully."));
    }

    [HttpGet("/api/instructor/my-tracks")]
    [Authorize(Roles = "Instructor")]
    public async Task<IActionResult> GetMyTracks()
    {
        var instructorIdClaim = User.FindFirst("InstructorId")?.Value;
        if (!int.TryParse(instructorIdClaim, out var instructorId))
        {
            return Unauthorized();
        }
        var tracks = await _instructorService.GetTracksAsync(instructorId);
        return Ok(ApiResponse<List<TrainingTrackListItemResponse>>.SuccessResponse(tracks,"My tracks retrieved successfully."));
    }
}