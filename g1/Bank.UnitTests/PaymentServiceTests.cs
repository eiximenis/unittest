using Bank.UnitTests.Factories;
using Moq;
using FluentAssertions;

namespace Bank.UnitTests
{
    public class PaymentServiceTests
    {
        private readonly PaymentService _svc;
        private readonly BankAccount _source;
        private readonly BankAccount _target;
        private readonly BankAccount _blockedSource;

        private readonly Mock<IEmailService> _emailServiceMock;
        

        private const string FromIban = "0000000";
        private const string BlockedFromIban = "0000001";
        private const string ToIban = "1111111";

        public PaymentServiceTests()
        {
            _emailServiceMock = new Mock<IEmailService>();

            var repoMock = new Mock<IBankAccountRepository>();
            _svc = new PaymentService(repoMock.Object, _emailServiceMock.Object);

            _source = new BankAccount(200m);
            _target = new BankAccount(330m);
            _blockedSource = new BankAccount(200m) { IsBlocked = true };

            repoMock.Setup(m => m.GetByIban(FromIban))
                .Returns(_source);
            repoMock.Setup(m => m.GetByIban(ToIban))
                .Returns(_target);
            repoMock.Setup(m => m.GetByIban(BlockedFromIban))
                .Returns(_blockedSource);


        }

        [Fact]
        public void GivenTwoValidIbansThenTransferShouldTransferTheMoney()
        {
            var iban1 = "1";
            var iban2 = "2";
            var sourceInitialMoney = 200m;
            var targetInitialMoney = 330m; ;
            var source = BankAccountFactory.CreateNew(sourceInitialMoney, blocked: false);
            var target = BankAccountFactory.CreateNew(targetInitialMoney, blocked: false);
            var mock = BankAccountRepositoryMockFactory.CreateWithTwoAccounts(iban1, iban2,source, target);
            var svc = new PaymentService(mock.Object, new Mock<IEmailService>().Object);
            // Arrange
            var amount = 100m;
            // Act
            var transferOk = svc.Transfer(iban1, iban2, new TransferData(amount, "Test transfer", sendEmail: false));
            
            // Assert
            var expectedSourceMoney = sourceInitialMoney - amount;
            var expectedTargetMoney = targetInitialMoney + amount;
            // Assert.True(transferOk);
            // Assert.Equal(expectedSourceMoney, source.Money);
            // Assert.Equal(expectedTargetMoney, target.Money);

            transferOk.Should().BeTrue();
            source.Money.Should().Be(expectedSourceMoney);
            target.Money.Should().Be(expectedTargetMoney);
            
        }

        [Fact]
        public void GivenAmountBiggerThanSourceMoneyThanTransferShouldReturnFalse()
        {
            var amount = 250m;

            var transferOk = _svc.Transfer(FromIban, ToIban, new TransferData(amount, "Test transfer", sendEmail: false));

            Assert.False(transferOk);
            Assert.Equal(200m, _source.Money);
            Assert.Equal(330m, _target.Money);
        }

        [Fact]
        public void GivenABlockedSourceAccountThenTransferShouldNotBeDone()
        {
            var amount = 100m;

            var transferOk = _svc.Transfer(BlockedFromIban, ToIban, new TransferData(amount, "Test transfer", sendEmail: false));

            Assert.False(transferOk);
            Assert.Equal(200m, _source.Money);
            Assert.Equal(330m, _target.Money);
        }

        [Fact]
        public void GivenAValidTransferWithSendEmailEnabledThenEmailShouldBeSent()
        {
            var transferOk = _svc.Transfer(FromIban, ToIban, 
                new TransferData(100m, "Test transfer", sendEmail: true));


            Assert.True(transferOk);
            _emailServiceMock.Verify(
                m => m.SendEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void GivenAnInvalidTransferThenEmailShouldNotBeSent()
        {
            var amount = 250m;
            var transferOk = _svc.Transfer(FromIban, ToIban, new TransferData(amount, "Test transfer", sendEmail: true));

            Assert.False(transferOk);
            _emailServiceMock.Verify(
                m => m.SendEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);

        }
    }
}
