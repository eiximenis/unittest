using Microsoft.AspNetCore.Mvc;

namespace Bank.FraudApi.Controllers
{
    public record FraudStatusResponse(string Iban, bool Fraud);

    [ApiController]
    [Route("[controller]")]
    public class FraudsController : ControllerBase
    {
        private readonly IFraudService _fraudService;
        public FraudsController(IFraudService fraudService)
        {
            _fraudService = fraudService;
        }

        [HttpPost("{iban}")]
        public async Task<IActionResult> AddFraudStatus(string iban)
        {
            await _fraudService.SetFraud(iban);
            return Accepted();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteFraudStatus(string iban)
        {
            await _fraudService.DeleteFraud(iban);
            return NoContent();
        }

        [HttpGet("{iban}")]
        public async Task<IActionResult> CheckFraudStatus(string iban)
        {
            var hasFraud = await _fraudService.CheckFraud(iban);
            return Ok(new FraudStatusResponse(iban, hasFraud));
        }

    }
}
