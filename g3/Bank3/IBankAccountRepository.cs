using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank3
{
    public interface IBankAccountRepository
    {
        BankAccount GetByIban(string iban);
    }
}
