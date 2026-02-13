using ISO20022.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ISO20022_processor_net10.Controllers
{
    [Route("api/[controller]/[action]")]
    public class ValidatorController : Controller
    {
        private readonly IXmlISOValidator _xmlISOValidator;

        public ValidatorController(IXmlISOValidator xmlISOValidator)
        {
            _xmlISOValidator = xmlISOValidator;
        }

        [HttpPost]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetSchemaUrns()
        {
            IEnumerable<string> urns = _xmlISOValidator.GetSchemaUrns();

            return Json(urns);
        }
    }
}