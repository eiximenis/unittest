using Bank.UnitTests.Factories;
using Moq;

namespace Bank.UnitTests
{
    public class PaymentServiceTests
    {
        private readonly Mock<IBankAccountRepository> _repoMock;
        private readonly PaymentService _svc;
        private readonly BankAccount _source;
        private readonly BankAccount _target;
        private readonly BankAccount _blockedSource;

        private const string FromIban = "0000000";
        private const string BlockedFromIban = "0000001";
        private const string ToIban = "1111111";

        public PaymentServiceTests()
        {
            _repoMock = new Mock<IBankAccountRepository>();
            _svc = new PaymentService(_repoMock.Object);

            _source = new BankAccount(200m);
            _target = new BankAccount(330m);
            _blockedSource = new BankAccount(200m) { IsBlocked = true };

            _repoMock.Setup(m => m.GetByIban(FromIban))
                .Returns(_source);
            _repoMock.Setup(m => m.GetByIban(ToIban))
                .Returns(_target);
            _repoMock.Setup(m => m.GetByIban(BlockedFromIban))
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
            var svc = new PaymentService(mock.Object);
            // Arrange
            var amount = 100m;
            // Act
            var transferOk = svc.Transfer(iban1, iban2, new TransferData(amount, "Test transfer"));
            
            // Assert
            var expectedSourceMoney = sourceInitialMoney - amount;
            var expectedTargetMoney = targetInitialMoney + amount;
            Assert.True(transferOk);
            Assert.Equal(expectedSourceMoney, source.Money);
            Assert.Equal(expectedTargetMoney, target.Money);

        }

        [Fact]
        public void GivenAmountBiggerThanSourceMoneyThanTransferShouldReturnFalse()
        {
            var amount = 250m;

            var transferOk = _svc.Transfer(FromIban, ToIban, new TransferData(amount, "Test transfer"));

            Assert.False(transferOk);
            Assert.Equal(200m, _source.Money);
            Assert.Equal(330m, _target.Money);
        }

        [Fact]
        public void GivenABlockedSourceAccountThenTransferShouldNotBeDone()
        {
            var amount = 100m;

            var transferOk = _svc.Transfer(BlockedFromIban, ToIban, new TransferData(amount, "Test transfer"));

            Assert.False(transferOk);
            Assert.Equal(200m, _source.Money);
            Assert.Equal(330m, _target.Money);
        }
    }
}
