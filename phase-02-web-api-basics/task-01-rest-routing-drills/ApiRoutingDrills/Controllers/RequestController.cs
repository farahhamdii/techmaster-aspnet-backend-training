using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace rest_routing_drills.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        [HttpGet("request-info")]
        public IActionResult GetRequestInfo()
        {
            var studentName = Request.Headers["X-Student-Name"].FirstOrDefault();

            if (string.IsNullOrWhiteSpace(studentName))
            {
                return BadRequest(new { message = "Header 'X-Student-Name' is missing" }); // 400 Bad Request
            }

            return Ok(new
            {
                studentName = studentName,
                path = Request.Path.Value
            });
        }
    }
}