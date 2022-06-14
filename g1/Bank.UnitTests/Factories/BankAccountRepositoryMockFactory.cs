using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;

namespace Bank.UnitTests.Factories
{
    internal class BankAccountRepositoryMockFactory
    {
        public static Mock<IBankAccountRepository> CreateWithTwoAccounts(
            string iban1, string iban2, BankAccount one, BankAccount two)
        {
            var mock = new Mock<IBankAccountRepository>();
            mock.Setup(m => m.GetByIban(iban1))
               .Returns(one);
            mock.Setup(m => m.GetByIban(iban2))
                .Returns(two);

            return mock;

            
        }
    }
}
