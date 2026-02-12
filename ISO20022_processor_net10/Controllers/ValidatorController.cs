using Microsoft.AspNetCore.Mvc;

namespace ISO20022_processor_net10.Controllers
{
    [Route("api/[controller]/[action]")]
    public class ValidatorController : Controller
    {
        [HttpPost]
        public IActionResult Index()
        {
            return View();
        }
    }
}