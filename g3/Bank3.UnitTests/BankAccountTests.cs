namespace Bank3.UnitTests
{
    public class BankAccountTests
    {
        private readonly BankAccount _account;
        public BankAccountTests()
        {
            _account = new BankAccount();
        }

        [Fact]
        public void GivenAnEmptyBankAccountItsMoneyShouldBeZero()
        {
            var actual = _account.Money;
            var expected = 0m;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GivenAnAmmountDepositShouldIncreaseMoney()
        {
            var ammount = 100m;
            _account.Deposit(ammount);

            var actual = _account.Money;
            var expected = ammount;
            Assert.Equal(expected, actual);
            
        }

        [Fact]
        public void GivenNegativeAmmountDepositShouldThrowAnException()
        {
            var ammount = -100m;
            Assert.Throws<ArgumentException>(() => _account.Deposit(ammount));
        }
    }
}