using Bank2.UnitTests.Factories;
using Moq;

namespace Bank2.UnitTests
{
    public class PaymentServiceTests
    {
        private readonly Mock<IBankAccountRepository> _repoMock;
        private readonly BankAccount _source;
        private readonly BankAccount _target;
        private readonly BankAccount _blockedSource;
        private readonly PaymentService _svc;

        public PaymentServiceTests()
        {
            _repoMock = new Mock<IBankAccountRepository>();
            _source = new BankAccount(200m);
            _target = new BankAccount(100m);
            _blockedSource = new BankAccount(200m) { IsBlocked = true };
            _repoMock.Setup(m => m.GetByIban("1234")).Returns(_source);
            _repoMock.Setup(m => m.GetByIban("9876")).Returns(_target);
            _repoMock.Setup(m => m.GetByIban("0000")).Returns(_blockedSource);
            _svc = new PaymentService(_repoMock.Object);
        }

        [Fact]
        public void GivenValidBankAccountsAndAmountThenPaymentServiceShouldDoTheTransfer()
        {
            // Arrange
            var scenario = BankAccountRepositoryMockFactory.CreateValidTransferScenario();
            var svc = new PaymentService(scenario.Mock.Object);

            // Act
            var transferOk = svc.Transfer(BankAccountTestsTransferScenario.IbanSource, 
                BankAccountTestsTransferScenario.IbanTarget, new TransferData(100m, "test transfer"));

            // Assert
            Assert.True(transferOk);
            Assert.Equal(100m, scenario.Source.Money);
            Assert.Equal(200m, scenario.Target.Money);
        }

        [Fact]
        public void GivenInsufficientAmountInSourceThenTransferShouldNotBeDone()
        {
            // Arrange

            // Act
            var transferOk = _svc.Transfer("1234", "9876", new TransferData(300m, "test transfer"));

            // Assert
            Assert.False(transferOk);
            Assert.Equal(200m, _source.Money);
            Assert.Equal(100m, _target.Money);
        }

        [Fact]
        public void GivenBlockedSourceAccountThenTransferShouldNotBeDone()
        {
            var transferOk = _svc.Transfer("0000", "9876", new TransferData(100m, "test transfer"));

            // Assert
            Assert.False(transferOk);
            Assert.Equal(200m, _blockedSource.Money);
            Assert.Equal(100m, _target.Money);

        }
    }
}
