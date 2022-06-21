using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransferController : ControllerBase
    {
        private readonly IMediator _mediator;
        public TransferController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> NewTransfer([FromBody] NewTransferRequest request)
        {
            var result = await _mediator.Send(request);
            return result ? Ok(result) : BadRequest();
        }            
    }
}
