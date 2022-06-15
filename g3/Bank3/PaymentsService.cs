using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank3
{
    public class PaymentsService : IPaymentsService
    {
        private readonly IBankAccountRepository _repo;
        private readonly IEmailService _emailService;
        public PaymentsService(IBankAccountRepository repo, IEmailService emailService)
        {
            ArgumentNullException.ThrowIfNull(emailService);
            _repo = repo;
            _emailService = emailService;
        }

        public TransferResult Transfer(string ibanFrom, string ibanTo, TransferData data)
        {
            var from = _repo.GetByIban(ibanFrom);
            var target = _repo.GetByIban(ibanTo);

            if (from.IsBlocked)
            {
                return TransferResult.BlockedAccount;
            }

            if (from.Money < data.Amount)
            {
                return TransferResult.NotEnoughFunds;
            }

            from.Withdraw(data.Amount);
            target.Deposit(data.Amount);

            if (data.SendEmail)
            {
                _emailService.SendEmail("customer@mail.com", "Transfer", $"You have received {data.Amount}");
            }

            return TransferResult.Ok;
        }
    }
}
