using IbanNet;
using IbanNet.Registry;
using Microsoft.AspNetCore.Mvc;

namespace ISO20022_processor_net10.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]

    public class IbanCheckController : Controller
    {
        private readonly IIbanValidator _ibanValidator;
        private readonly IIbanGenerator _ibanGenerator;
        private readonly IIbanRegistry _ibanRegistry;

        public IbanCheckController(IIbanValidator  ibanValidator,
            IIbanGenerator ibanGenerator,
            IIbanRegistry ibanRegistry)
        {
            _ibanValidator = ibanValidator;
            _ibanGenerator = ibanGenerator;
            _ibanRegistry = ibanRegistry;
        }

        [HttpGet]
        public IActionResult Index(string iban)
        {
            ArgumentException.ThrowIfNullOrEmpty(iban);

            var result = _ibanValidator.Validate(iban);
            if (!result.IsValid)
            {
                return BadRequest(new { error = "Invalid IBAN" });
            }

            return Ok(result);
        }

        [HttpGet]
        public IActionResult IbanGenerator(string countryCode)
        {
            ArgumentException.ThrowIfNullOrEmpty(countryCode);

            return Ok(_ibanGenerator.Generate(countryCode));
        }

        [HttpGet]
        public IActionResult IbanSupportedContries()
        {
            return Ok(_ibanRegistry);
        }

        [HttpGet]
        public IActionResult UkAccntToIbanCalc(string bic, string accountNr, string sortcode)
        {
            ArgumentException.ThrowIfNullOrEmpty(bic);
            ArgumentException.ThrowIfNullOrEmpty(accountNr);
            ArgumentException.ThrowIfNullOrEmpty(sortcode);

            bic = bic.Substring(0, 4);



            return Ok("IBAN");
        }
    }
}