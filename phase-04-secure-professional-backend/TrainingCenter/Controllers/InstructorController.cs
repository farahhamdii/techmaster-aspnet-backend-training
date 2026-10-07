
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

        return Ok(new ApiResponse<List<InstructorListItemResponse>>
        {
            Success = true,
            Message = "Instructors retrieved successfully.",
            Data = instructors
        });
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
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = "Instructor not found."
            });
        }

        return Ok(new ApiResponse<InstructorDetailsResponse>
        {
            Success = true,
            Message = "Instructor retrieved successfully.",
            Data = instructor
        });
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
                new ApiResponse<InstructorListItemResponse>
                {
                    Success = true,
                    Message = "Instructor created successfully.",
                    Data = instructor
                });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    // Admin only
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(
        int id,
        UpdateInstructorRequest request)
    {
        try
        {
            var instructor = await _instructorService.UpdateAsync(id, request);

            if (instructor == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Instructor not found."
                });
            }

            return Ok(new ApiResponse<InstructorListItemResponse>
            {
                Success = true,
                Message = "Instructor updated successfully.",
                Data = instructor
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = ex.Message
            });
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

        return Ok(new ApiResponse<List<TrainingTrackListItemResponse>>
        {
            Success = true,
            Message = "Instructor tracks retrieved successfully.",
            Data = tracks
        });
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

        return Ok(new ApiResponse<List<TrainingTrackListItemResponse>>
        {
            Success = true,
            Message = "My tracks retrieved successfully.",
            Data = tracks
        });
    }

}
