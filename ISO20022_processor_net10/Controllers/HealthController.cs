using ISO20022_processor_net10.Base;
using Microsoft.AspNetCore.Mvc;

namespace ISO20022_processor_net10.Controllers
{
    public class HealthController : BaseController
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Ok("OK");
        }
    }
}