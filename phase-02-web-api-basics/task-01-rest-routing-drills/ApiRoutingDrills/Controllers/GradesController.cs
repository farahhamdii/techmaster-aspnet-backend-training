using Microsoft.AspNetCore.Mvc;

namespace ApiRoutingDrills.Controllers
{
    [ApiController]
    [Route("api/grades")]
    public class GradesController : ControllerBase
    {
        // GET /api/grades/calculate?score=85
        [HttpGet("calculate")]
        public IActionResult CalculateGrade([FromQuery] double score)
        {
            if (score < 0 || score > 100)
            {
                return BadRequest(new { error = "Score must be between 0 and 100" });
            }

            string grade = score switch
            {
                >= 90 => "A",
                >= 80 => "B",
                >= 70 => "C",
                >= 60 => "D",
                _ => "F"
            };

            bool isPassed = score >= 60;

            return Ok(new
            {
                score = score,
                grade = grade,
                isPassed = isPassed,
                status = isPassed ? "Passed" : "Failed"
            });
        }
    }
}