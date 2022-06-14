namespace Bank2.UnitTests
{
    public class BankAccountTests
    {
        private readonly BankAccount _account;

        public BankAccountTests()
        {
            _account = new BankAccount();
        }
        

        [Fact]
        public void GivenAnNewBankAccountItsMoneyShouldBeZero()
        {
            var expectedResult = 0m;
            var actualResult = _account.Money;

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void GivenAnAmmountOfMoneyDepositShoudIncrementAccount()
        {
            var moneyToDeposit = 100m;
            var initialMoney = _account.Money;
            _account.Deposit(moneyToDeposit);
            var expectedResult = initialMoney + moneyToDeposit;
            var actualResult = _account.Money;

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void GivenANegativeAmmountOfMoneyDepositShouldThrowAnException()
        {
            Assert.Throws<ArgumentException>(() => _account.Deposit(-100m));
        }
    }
}