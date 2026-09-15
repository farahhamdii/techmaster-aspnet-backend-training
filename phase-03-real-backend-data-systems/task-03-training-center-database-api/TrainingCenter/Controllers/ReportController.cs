
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
}