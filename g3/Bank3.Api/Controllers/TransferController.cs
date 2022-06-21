using Bank3.Api.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bank3.Api.Controllers
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
            return result switch
            {
                TransferResult.Ok => Ok(result),
                TransferResult.BlockedAccount or TransferResult.NotEnoughFunds => BadRequest(result),
                _ => throw new NotImplementedException()

            };
            
        }
    }
}
