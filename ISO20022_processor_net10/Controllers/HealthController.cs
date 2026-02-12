using Microsoft.AspNetCore.Mvc;

namespace ISO20022_processor_net10.Controllers
{
    [Route("api/[controller]/[action]")]
    public class HealthController : Controller
    {
        [Route("/api/health")]
        public IActionResult Index()
        {
            return Ok("OK");
        }
    }
}