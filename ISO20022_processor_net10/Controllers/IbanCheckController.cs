using Microsoft.AspNetCore.Mvc;

namespace ISO20022_processor_net10.Controllers
{
    [Route("api/[controller]/[action]")]
    public class IbanCheckController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}