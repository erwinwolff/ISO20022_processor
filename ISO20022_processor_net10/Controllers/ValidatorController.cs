using ISO20022.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Xml.Serialization;

namespace ISO20022_processor_net10.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]

    public class ValidatorController : Controller
    {
        private readonly IXmlISOValidator _xmlISOValidator;

        public ValidatorController(IXmlISOValidator xmlISOValidator)
        {
            _xmlISOValidator = xmlISOValidator;
        }

        [HttpPost]
        public async Task<IActionResult> ValidateXml(string xml)
        {
            ArgumentException.ThrowIfNullOrEmpty(xml);

            var validationResult = await _xmlISOValidator.AutomaticValidationAsync(xml);

            return Json(new { valid = validationResult.Item1, message = validationResult.Item2 });
        }

        [HttpGet]
        public IActionResult GetSchemaUrns()
        {
            IEnumerable<string> urns = _xmlISOValidator.GetSchemaUrns();

            return Json(urns);
        }

        [HttpGet]
        public IActionResult GetXmlByUrl(string urn)
        {
            ArgumentException.ThrowIfNullOrEmpty(urn);

            if (!_xmlISOValidator.GetSchemaUrns().Any(x => x == urn))
                return NotFound();

            var type = _xmlISOValidator.SchemaToType(urn);
            var xmlDef = Activator.CreateInstance(type);

            xmlDef.InflateXmlPocoDefinition();

            string entireDefinition = string.Empty;

            XmlSerializer xmlSerializer = new XmlSerializer(type);
            using (MemoryStream mem = new MemoryStream())
            {
                xmlSerializer.Serialize(mem, xmlDef);
                entireDefinition = UTF8Encoding.UTF8.GetString(mem.ToArray());
            }

            return Json(new { xmlDef = entireDefinition });
            
        }
    }
}