using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace rest_routing_drills.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToolsController : ControllerBase
    {
        [HttpGet("echo/{name}")]
        public IActionResult Echo(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest(new { message = "Name parameter cannot be empty." });
            }
            return Ok(new
            {
                originalName = name,
                message = $"Hello, {name}!"
            });
        }
    }
}
