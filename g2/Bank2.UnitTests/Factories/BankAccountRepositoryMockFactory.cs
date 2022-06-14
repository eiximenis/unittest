using Moq;

namespace Bank2.UnitTests.Factories
{
    class BankAccountTestsTransferScenario
    {
        public const string IbanSource = "1111";
        public const string IbanTarget = "0000";

        public BankAccount Source { get; init; }
        public BankAccount Target { get; init; }
        public Mock<IBankAccountRepository> Mock { get; init; }
        

    }

    class BankAccountRepositoryMockFactory
    {
        public static BankAccountTestsTransferScenario CreateValidTransferScenario()
        {
            var repoMock = new Mock<IBankAccountRepository>();
            var source = new BankAccount(200m);
            var target = new BankAccount(100m);
            repoMock.Setup(m => m.GetByIban(BankAccountTestsTransferScenario.IbanSource)).Returns(source);
            repoMock.Setup(m => m.GetByIban(BankAccountTestsTransferScenario.IbanTarget)).Returns(target);

            return new BankAccountTestsTransferScenario()
            {
                Mock = repoMock,
                Source = source,
                Target = target
            };
        }
    }
}
