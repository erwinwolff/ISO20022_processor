using Microsoft.AspNetCore.Mvc;

namespace ISO20022_processor_net10.Controllers
{
    public class HomeController : Controller
    {
        [Route("/")]
        [Route("index.html")]
        [Route("default.aspx")]
        public IActionResult Index()
        {
            return View();
        }
    }
}