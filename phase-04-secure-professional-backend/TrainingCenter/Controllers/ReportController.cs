
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrainingCenter.Common;
using TrainingCenter.DTOs;
using TrainingCenter.Services;

namespace TrainingCenter.Controllers;

[ApiController]
[Route("api/reports")]
[Authorize(Roles = "Admin")]
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
        var result = await _reportService.GetDashboardSummaryAsync();
        return Ok(ApiResponse<DashboardSummaryResponse>.SuccessResponse(result,"Dashboard summary retrieved successfully."));
    }

    [HttpGet("unpaid-enrollments")]
    public async Task<IActionResult> GetUnpaidEnrollments()
    {
       var result = await _reportService.GetUnpaidEnrollmentsAsync();
        return Ok(ApiResponse<List<UnpaidEnrollmentResponse>>.SuccessResponse(
                result,"Unpaid enrollments retrieved successfully."));
    }

    [HttpGet("track-capacity")]
    public async Task<IActionResult> GetTrackCapacity()
    {
        var result = await _reportService.GetTrackCapacityAsync();
        return Ok(ApiResponse<List<TrackCapacityResponse>>.SuccessResponse(
                result, "Track capacity retrieved successfully."));
    }

    [HttpGet("revenue-summary")]
    public async Task<IActionResult> GetRevenueSummary()
    {
        var result = await _reportService.GetRevenueSummaryAsync();
        return Ok(ApiResponse<RevenueSummaryResponse>.SuccessResponse(
                result,"Revenue summary retrieved successfully."));
    }

    [HttpGet("revenue-by-track")]
    public async Task<IActionResult> GetRevenueByTrack()
    {
        var result = await _reportService.GetRevenueByTrackAsync();
        return Ok(ApiResponse<List<RevenueByTrackResponse>>.SuccessResponse(
                result, "Revenue by track retrieved successfully."));
    }

    [HttpGet("tracks-with-available-seats")]
    public async Task<IActionResult> GetTracksWithAvailableSeats()
    {
        var tracks =await _reportService.GetTracksWithAvailableSeatsAsync();
        return Ok( ApiResponse<List<TrackAvailableSeatsResponse>>.SuccessResponse(
                tracks,"Tracks with available seats retrieved successfully."));
    }

    [HttpGet("top-tracks")]
    public async Task<IActionResult> GetTopTracks(int top = 5)
    {
        try
        {
            var result = await _reportService.GetTopTracksAsync(top);
            return Ok(
                ApiResponse<List<TopTrackResponse>>.SuccessResponse( result, "Top tracks retrieved successfully."));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
        }
    }

    [HttpGet("instructor-workload")]
    public async Task<IActionResult> GetInstructorWorkload()
    {
        var result = await _reportService.GetInstructorWorkloadAsync();
        return Ok(ApiResponse<List<InstructorWorkloadResponse>>.SuccessResponse(
                result,"Instructor workload retrieved successfully."));
    }

    [HttpGet("students-without-payments")]
    public async Task<IActionResult> GetStudentsWithoutPayments()
    {
        var result =await _reportService.GetStudentsWithoutPaymentsAsync();
        return Ok(ApiResponse<List<StudentWithoutPaymentResponse>>.SuccessResponse(
                result, "Students without payments retrieved successfully."));
    }
}
