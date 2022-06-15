using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    public class PaymentService : IPaymentService
    {

        private readonly IBankAccountRepository _repository;
        private readonly IEmailService _emailService;
        public PaymentService(IBankAccountRepository repository, IEmailService emailService)
        {
            ArgumentNullException.ThrowIfNull(emailService);
            _repository = repository;
            _emailService = emailService;
        }
        public bool Transfer(string from, string to, TransferData data)
        {
            var source = _repository.GetByIban(from);
            var target = _repository.GetByIban(to);
            var qty = data.amount;

            if (source.Money < data.amount)
            {
                return false;
            }

            if (source.IsBlocked)
            {
                return false;
            }

            source.Withdrawn(qty);
            target.Ingress(qty);

            if (data.sendEmail)
            {
                _emailService.SendEmail("someone@mail.com", "New Transfer!", "");
            }

            return true;
        }
    }
}
