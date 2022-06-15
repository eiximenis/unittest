using Bank2.UnitTests.Factories;
using Moq;
using FluentAssertions;

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
            _svc = new PaymentService(_repoMock.Object, new Mock<IEmailService>().Object);
        }

        [Fact]
        public void GivenValidBankAccountsAndAmountThenPaymentServiceShouldDoTheTransfer()
        {
            // Arrange
            var scenario = BankAccountRepositoryMockFactory.CreateValidTransferScenario();
            var svc = new PaymentService(scenario.Mock.Object, new Mock<IEmailService>().Object);

            // Act
            var transferOk = svc.Transfer(BankAccountTestsTransferScenario.IbanSource, 
                BankAccountTestsTransferScenario.IbanTarget, new TransferData(100m, "test transfer", SendEmail: false));

            // Assert
            // Assert.True(transferOk);
            Assert.Equal(100m, scenario.Source.Money);
            Assert.Equal(200m, scenario.Target.Money);

            transferOk.Should().BeTrue();
            scenario.Target.Money.Should().Be(200m);
        }

        [Fact]
        public void GivenValidBankAccountsAndAmountThenPaymentServiceShouldSendEmailIfRequested()
        {
            var scenario = BankAccountRepositoryMockFactory.CreateValidTransferScenario();
            var emailMock = new Mock<IEmailService>();
            var svc = new PaymentService(scenario.Mock.Object,emailMock.Object);
            
            var transferOk = svc.Transfer(BankAccountTestsTransferScenario.IbanSource,
                BankAccountTestsTransferScenario.IbanTarget, new TransferData(100m, "test transfer", SendEmail: true));


            Assert.True(transferOk);
            emailMock.Verify(x => x.SendEmail(It.IsAny<string>(), "You received 100", ""));
        }

        [Fact]
        public void GivenInsufficientAmountInSourceThenTransferShouldNotBeDone()
        {
            // Arrange

            // Act
            var transferOk = _svc.Transfer("1234", "9876", new TransferData(300m, "test transfer", SendEmail: false));

            // Assert
            Assert.False(transferOk);
            Assert.Equal(200m, _source.Money);
            Assert.Equal(100m, _target.Money);
        }

        [Fact]
        public void GivenInvalidTransferThenPaymentServiceShouldNotSendAnEmail()
        {
            var scenario = BankAccountRepositoryMockFactory.CreateValidTransferScenario();
            var emailMock = new Mock<IEmailService>();
            var svc = new PaymentService(scenario.Mock.Object, emailMock.Object);
            var transferOk = svc.Transfer(BankAccountTestsTransferScenario.IbanSource,
                BankAccountTestsTransferScenario.IbanTarget, new TransferData(1000m, "test transfer", SendEmail: true));

            Assert.False(transferOk);
            emailMock.Verify(
                x => x.SendEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            
        }

        [Fact]
        public void GivenBlockedSourceAccountThenTransferShouldNotBeDone()
        {
            var transferOk = _svc.Transfer("0000", "9876", new TransferData(100m, "test transfer", SendEmail: false));

            // Assert
            Assert.False(transferOk);
            Assert.Equal(200m, _blockedSource.Money);
            Assert.Equal(100m, _target.Money);

        }
    }
}
