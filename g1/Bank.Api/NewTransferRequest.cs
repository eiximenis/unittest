using MediatR;

namespace Bank.Api
{
    public class NewTransferRequest : IRequest<bool>
    {
        public string From { get; set; }
        public string To { get; set; }
        public decimal Amount { get; set; }

        public string Concept { get; set; }

        public bool SendEmail { get; set; }
    }


    public class NewTransferRequestHandler : IRequestHandler<NewTransferRequest, bool>
    {

        private readonly IPaymentService _svc;
        public NewTransferRequestHandler(IPaymentService svc)
        {
            _svc = svc;
        }
        public Task<bool> Handle(NewTransferRequest request, CancellationToken cancellationToken)
        {
            var result = _svc.Transfer(request.From, request.To,
                new TransferData(request.Amount, request.Concept, request.SendEmail));
            return Task.FromResult(result);
        }
    }
}
