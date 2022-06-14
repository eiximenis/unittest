using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.UnitTests.Factories
{
    class BankAccountFactory
    {
        public static BankAccount CreateNew(decimal import, bool blocked) 
            => new BankAccount(import) { IsBlocked = blocked };
    }
}
