using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ISO20022_processor_net10.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        public IActionResult Get()
        {
            return Ok("Healthy");
        }
    }
}