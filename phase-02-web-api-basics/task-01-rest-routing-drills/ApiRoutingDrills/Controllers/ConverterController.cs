using Microsoft.AspNetCore.Mvc;
using ApiRoutingDrills.Services;

namespace ApiRoutingDrills.Controllers
{
    [ApiController]
    [Route("api/converter")]
    public class ConverterController : ControllerBase
    {
        private readonly IConverterService _converterService;
        public ConverterController(IConverterService converterService)
        {
            _converterService = converterService;
        }

        // GET /api/converter/celsius-to-fahrenheit?value=25
        [HttpGet("celsius-to-fahrenheit")]
        public IActionResult CelsiusToFahrenheit([FromQuery] decimal value)
        {
            var result = _converterService.ConvertCelsiusToFahrenheit(value);
            return Ok(result);
        }
    }
}