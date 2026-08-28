using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace rest_routing_drills.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetHealth()
        {
            return Ok(new
            {
                status = "Running",
                service = "TechMaster API",
                time = DateTime.UtcNow
            });
        }
    }
}
