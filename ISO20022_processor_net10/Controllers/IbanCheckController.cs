using Microsoft.AspNetCore.Mvc;

namespace ISO20022_processor_net10.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]

    public class IbanCheckController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Json(new { });
        }
    }
}