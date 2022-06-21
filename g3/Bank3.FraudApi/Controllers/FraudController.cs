using Microsoft.AspNetCore.Mvc;

namespace Bank3.FraudApi.Controllers
{
    public record FraudStatusResponse(string Iban, bool Fraud);

    [ApiController]
    [Route("[controller]")]
    public class FraudController : ControllerBase
    {
        private readonly IFraudService _fraudService;
        public FraudController(IFraudService fraudService)
        {
            _fraudService = fraudService;
        }

        [HttpGet("{iban}")]
        public async Task<IActionResult> CheckFraud(string iban)
        {
            var isFraud = await _fraudService.CheckFraud(iban);
            return Ok(new FraudStatusResponse(iban, isFraud));
        }
    }
}
