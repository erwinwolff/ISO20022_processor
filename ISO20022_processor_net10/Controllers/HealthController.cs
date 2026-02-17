using Microsoft.AspNetCore.Mvc;

namespace ISO20022_processor_net10.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]

    public class HealthController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Ok("OK");
        }
    }
}