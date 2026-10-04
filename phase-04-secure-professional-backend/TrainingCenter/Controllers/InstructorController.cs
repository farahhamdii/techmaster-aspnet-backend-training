using Microsoft.AspNetCore.Mvc;
using TrainingCenter.Common;
using TrainingCenter.DTOs;
using TrainingCenter.DTOs.Instructor;
using TrainingCenter.Services;

namespace TrainingCenter.Controllers;

[ApiController]
[Route("api/instructors")]
public class InstructorController : ControllerBase
{
    private readonly IInstructorService _instructorService;
    public InstructorController(IInstructorService instructorService)
    {
        _instructorService = instructorService;
    }

    [HttpGet]
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

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
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

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateInstructorRequest request)
    {
        try
        {
            var instructor =await _instructorService.CreateAsync(request);

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
            return Conflict(new ApiResponse<object>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id,UpdateInstructorRequest request)
    {
        try
        {
            var instructor =await _instructorService.UpdateAsync(id, request);
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

    [HttpGet("{id}/tracks")]
    public async Task<IActionResult> GetTracks(int id)
    {
        var instructor = await _instructorService.GetByIdAsync(id);

        if (instructor == null)
        {
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = "Instructor not found."
            });
        }

        var tracks = await _instructorService.GetTracksAsync(id);

        return Ok(new ApiResponse<List<TrainingTrackListItemResponse>>
        {
            Success = true,
            Message = "Instructor tracks retrieved successfully.",
            Data = tracks
        });
    }
}