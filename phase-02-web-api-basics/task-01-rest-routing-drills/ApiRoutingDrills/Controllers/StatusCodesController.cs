using Microsoft.AspNetCore.Mvc;

namespace ApiRoutingDrills.Controllers
{
    [ApiController]
    [Route("api/status-codes")]
    public class StatusCodesController : ControllerBase
    {
        [HttpGet("ok-demo")]
        public IActionResult GetOkDemo()
        {
            return Ok(new { message = "Resource retrieved successfully" });
        }

        [HttpPost("created-demo")]
        public IActionResult GetCreatedDemo()
        {
            return Created("/api/status-codes/created-demo/1", new { id = 1, status = "Resource Created" });
        }

        [HttpDelete("no-content-demo")]
        public IActionResult GetNoContentDemo()
        {
            return NoContent();
        }

        [HttpGet("bad-request-demo")]
        public IActionResult GetBadRequestDemo([FromQuery] string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest(new { message = "Name parameter is required" });
            }
            return Ok(new { message = $"Hello {name}" });
        }

        [HttpGet("not-found-demo/{id}")]
        public IActionResult GetNotFoundDemo(int id)
        {
            if (id != 1)
            {
                return NotFound(new { message = $"Item with ID {id} was not found" });
            }
            return Ok(new { id = 1, name = "Sample Item" });
        }
    }
}