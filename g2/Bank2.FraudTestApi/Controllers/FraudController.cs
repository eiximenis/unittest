using Microsoft.AspNetCore.Mvc;

namespace Bank2.FraudTestApi.Controllers
{
    
    public record FraudStatusResponse(string Iban, bool Fraud);

    [ApiController]
    [Route("[controller]")]
    public class FraudController : ControllerBase
    {
        private readonly IFraudService _service;
        public FraudController(IFraudService service)
        {
            _service = service;
        }

        [HttpGet("{iban}")]
        public async Task<IActionResult> CheckFraudStatus(string iban)
        {
            var hasFraud = await _service.CheckFraud(iban);
            return Ok(new FraudStatusResponse(iban, hasFraud));
        }


    }
}
