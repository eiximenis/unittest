using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    public class BankAccountRepository : IBankAccountRepository
    {
        public BankAccount GetByIban(string iban)
        {
            // PRODUCTION CODE
            return null!;
        }
    }
}
