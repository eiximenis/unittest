using MediatR;

namespace Bank3.Api.Requests
{
    public class NewTransferRequest : IRequest<TransferResult>
    {
        public string From { get; set; }
        public string To { get; set; }
        public decimal Amount { get; set; }

        public string Concept { get; set; }
        public bool SendEmail { get; set; }
    }
}
