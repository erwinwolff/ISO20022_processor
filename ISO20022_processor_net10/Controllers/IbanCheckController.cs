using IbanNet;
using IbanNet.Registry;
using ISO20022_processor_net10.Base;
using Microsoft.AspNetCore.Mvc;
using Wolff.FinanceTools.UK;

namespace ISO20022_processor_net10.Controllers
{
    public class IbanCheckController : BaseController
    {
        private readonly IIbanValidator _ibanValidator;
        private readonly IIbanGenerator _ibanGenerator;
        private readonly IIbanRegistry _ibanRegistry;
        private readonly IUkAccntNumberToIban _ukAccntNumberToIban;

        public IbanCheckController(IIbanValidator  ibanValidator,
            IIbanGenerator ibanGenerator,
            IIbanRegistry ibanRegistry,
            IUkAccntNumberToIban ukAccntNumberToIban)
        {
            _ibanValidator = ibanValidator;
            _ibanGenerator = ibanGenerator;
            _ibanRegistry = ibanRegistry;
            _ukAccntNumberToIban = ukAccntNumberToIban;
        }

        [HttpGet]
        public IActionResult Index([FromQuery] string iban)
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
        public IActionResult IbanGenerator([FromQuery] string countryCode)
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
        public IActionResult UkAccntToIbanCalc(
            [FromQuery] string bic,
            [FromQuery] string sortcode,
            [FromQuery] string accountNr)
        {
            ArgumentException.ThrowIfNullOrEmpty(bic);
            ArgumentException.ThrowIfNullOrEmpty(sortcode);
            ArgumentException.ThrowIfNullOrEmpty(accountNr);

            return Ok(_ukAccntNumberToIban.Convert(bic, sortcode, accountNr));
        }
    }
}