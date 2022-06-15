using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank2
{
    public class PaymentService : IPaymentService
    {
        private readonly IBankAccountRepository _repo;
        private readonly IEmailService _emailService;

        public PaymentService(IBankAccountRepository repo, IEmailService emailService)
        {
            ArgumentNullException.ThrowIfNull(emailService);
            _repo = repo;
            _emailService = emailService;
        }
        public bool Transfer(string ibanFrom, string ibanTo, TransferData data)
        {
            var from = _repo.GetByIban(ibanFrom);
            var to = _repo.GetByIban(ibanTo);

            if (from.Money < data.Amount)
            {
                return false;
            }

            if (from.IsBlocked)
            {
                return false;
            }

            from.Withdraw(data.Amount);
            to.Deposit(data.Amount);
            
            if (data.SendEmail)
            {
                // var emailSvc = new EmailService();
                _emailService.SendEmail("someone@mail.com", $"You received {data.Amount}", "");
            }


            return true;
        }
    }
}
