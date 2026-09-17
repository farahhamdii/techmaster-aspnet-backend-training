
using Microsoft.AspNetCore.Mvc;
using TrainingCenter.Common;
using TrainingCenter.DTOs;
using TrainingCenter.Entities;
using TrainingCenter.Services;
using static System.Net.Mime.MediaTypeNames;

namespace TrainingCenter.Controllers;

[ApiController]
[Route("api/reports")]
public class ReportsController : ControllerBase
{
    private readonly IReportService _reportService;
    public ReportsController(IReportService reportService)
    {
        _reportService = reportService;
    }

    [HttpGet("dashboard-summary")]
    public async Task<IActionResult> GetDashboardSummary()
    {
        var result =await _reportService.GetDashboardSummaryAsync();

        return Ok(new ApiResponse<DashboardSummaryResponse>
        {
            Success = true,
            Message = "Dashboard summary retrieved successfully.",
            Data = result
        });
    }

    [HttpGet("unpaid-enrollments")]
    public async Task<IActionResult> GetUnpaidEnrollments()
    {
        var result =await _reportService.GetUnpaidEnrollmentsAsync();
        return Ok(new ApiResponse<List<UnpaidEnrollmentResponse>>
        {
            Success = true,
            Message = "Unpaid enrollments retrieved successfully.",
            Data = result
        });
    }

    [HttpGet("track-capacity")]
    public async Task<IActionResult> GetTrackCapacity()
    {
        var result =await _reportService.GetTrackCapacityAsync();
        return Ok(new ApiResponse<List<TrackCapacityResponse>>
        {
            Success = true,
            Message = "Track capacity retrieved successfully.",
            Data = result
        });
    }

    [HttpGet("revenue-summary")]
    public async Task<IActionResult> GetRevenueSummary()
    {
        var result =await _reportService.GetRevenueSummaryAsync();
        return Ok(new ApiResponse<RevenueSummaryResponse>
        {
            Success = true,
            Message = "Revenue summary retrieved successfully.",
            Data = result
        });
    }

    [HttpGet("revenue-by-track")]
    public async Task<IActionResult> GetRevenueByTrack()
    {
        var result =await _reportService.GetRevenueByTrackAsync();
        return Ok(new ApiResponse<List<RevenueByTrackResponse>>
        {
            Success = true,
            Message = "Revenue by track retrieved successfully.",
            Data = result
        });
    }
    [HttpGet("tracks-with-available-seats")]
    public async Task<IActionResult> GetTracksWithAvailableSeats()
    {
        var tracks = await _reportService
            .GetTracksWithAvailableSeatsAsync();

        return Ok(new ApiResponse<List<TrackAvailableSeatsResponse>>
        {
            Success = true,
            Message = "Tracks with available seats retrieved successfully.",
            Data = tracks
        });
    }
    [HttpGet("top-tracks")]
    public async Task<IActionResult> GetTopTracks(int top = 5)
    {
        try
        {
            var result = await _reportService.GetTopTracksAsync(top);

            return Ok(new ApiResponse<List<TopTrackResponse>>
            {
                Success = true,
                Message = "Top tracks retrieved successfully.",
                Data = result
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }
    [HttpGet("instructor-workload")]
    public async Task<IActionResult> GetInstructorWorkload()
    {
        var result = await _reportService.GetInstructorWorkloadAsync();

        return Ok(new ApiResponse<List<InstructorWorkloadResponse>>
        {
            Success = true,
            Message = "Instructor workload retrieved successfully.",
            Data = result
        });
    }

    [HttpGet("students-without-payments")]
    public async Task<IActionResult> GetStudentsWithoutPayments()
    {
        var result = await _reportService.GetStudentsWithoutPaymentsAsync();

        return Ok(new ApiResponse<List<StudentWithoutPaymentResponse>>
        {
            Success = true,
            Message = "Students without payments retrieved successfully.",
            Data = result
        });
    }
}