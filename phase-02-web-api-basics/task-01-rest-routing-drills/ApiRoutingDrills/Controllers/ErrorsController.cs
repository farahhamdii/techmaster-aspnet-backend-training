using Microsoft.AspNetCore.Mvc;

namespace ApiRoutingDrills.Controllers
{
    [ApiController]
    [Route("api/errors")]
    public class ErrorsController : ControllerBase
    {
        // Drill 15 Demo - Bad Request (400) Standard Error Shape
        [HttpGet("demo/bad-request")]
        public IActionResult GetBadRequestErrorDemo()
        {
            var errorResponse = new
            {
                success = false,
                code = 400,
                message = "Invalid request payload",
                errors = new[] { "Name field is required", "Age must be a positive number" },
                timestamp = DateTime.UtcNow
            };

            return BadRequest(errorResponse);
        }

        [HttpGet("demo/not-found")]
        public IActionResult GetNotFoundErrorDemo()
        {
            var errorResponse = new
            {
                success = false,
                code = 404,
                message = "Requested resource was not found",
                errors = new[] { "No entity matching ID 999 exists in the system" },
                timestamp = DateTime.UtcNow
            };

            return NotFound(errorResponse);
        }
    }
}