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
        public PaymentService(IBankAccountRepository repository)
        {
            _repository = repository;
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
            return true;
        }
    }
}
