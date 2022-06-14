using Bank3.UnitTests.Factories;
using Moq;

namespace Bank3.UnitTests
{
    public class PaymentsServiceTests
    {
        private readonly Mock<IBankAccountRepository> _mock;
        private readonly PaymentsService _svc;
        private readonly BankAccount _source;
        private readonly BankAccount _target;
        private readonly BankAccount _blockedSource;
        
        public PaymentsServiceTests()
        {
            _source = new BankAccount(200m);
            _blockedSource = new BankAccount(200m) { IsBlocked = true };
            _target = new BankAccount(300m);
            _mock = new Mock<IBankAccountRepository>();
            _mock.Setup(x => x.GetByIban("1111"))
                .Returns(_source);
            _mock.Setup(x => x.GetByIban("2222"))
                .Returns(_target);
            _mock.Setup(x => x.GetByIban("0000"))
                .Returns(_blockedSource);

            _svc = new PaymentsService(_mock.Object);
        }

        [Fact]
        public void GivenValidAmmountThenTransferShouldTransferTheMoney()
        {
            // Arrange
            var scenario = TransferTestsScenariosFactory.CreateScenarioWithTwoUnblockedAccounts();

            // Act
            var result = scenario.Service.Transfer("1111", "2222", new TransferData(100m, "Test transfer"));

            // Assert
            Assert.Equal(TransferResult.Ok, result);
            Assert.Equal(100m, scenario.Source.Money);
            Assert.Equal(400m, scenario.Target.Money);
        }

        [Fact]
        public void GivenSourceHasNotEnoughAmountThenTransferShouldNotBeDone()
        {
            // Arrange
            
            // Act
            var result = _svc.Transfer("1111", "2222", new TransferData(500m, "Test transfer"));

            // Assert
            Assert.Equal(TransferResult.NotEnoughFunds, result);
            Assert.Equal(200m, _source.Money);
            Assert.Equal(300m, _target.Money);
        }

        [Fact]
        public void GivenBlockedSourceThenTransferShouldNotBeDone()
        {
            var result = _svc.Transfer("0000", "2222", new TransferData(100m, "Test transfer"));

            // Assert
            Assert.Equal(TransferResult.BlockedAccount, result);
            Assert.Equal(200m, _blockedSource.Money);
            Assert.Equal(300m, _target.Money);

        }

    }
}
