using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank3.UnitTests.Factories
{
    class TransferTestsScenario
    {
        public BankAccount Source { get; init; }
        public BankAccount Target { get; init; }
        public PaymentsService Service { get; init; }
    }
    static class TransferTestsScenariosFactory
    {
        public static TransferTestsScenario CreateScenarioWithTwoUnblockedAccounts()
        {
            var source = new BankAccount(200m);
            var target = new BankAccount(300m);
            var mock = new Mock<IBankAccountRepository>();
            mock.Setup(x => x.GetByIban("1111"))
                .Returns(source);
            mock.Setup(x => x.GetByIban("2222"))
                .Returns(target);
            var svc = new PaymentsService(mock.Object);
            return new TransferTestsScenario()
            {
                Service = svc,
                Source = source,
                Target = target
            };
        }
    }
}
