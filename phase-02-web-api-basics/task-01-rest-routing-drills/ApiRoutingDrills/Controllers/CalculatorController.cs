using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace rest_routing_drills.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalculatorController : ControllerBase
    {
        [HttpGet("add")]
        public IActionResult Add([FromQuery] decimal a, [FromQuery] decimal b)
        {
            return Ok(new
            {
                a = a,
                b = b,
                operation = "add",
                result = a + b
            });
        }
    }
}
