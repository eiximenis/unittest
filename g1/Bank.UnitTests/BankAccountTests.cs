namespace Bank.UnitTests
{
    public class BankAccountShould
    {
        private readonly BankAccount _account;
        public BankAccountShould()
        {
            _account = new BankAccount();
        }

        [Fact]
        public void BeCreatedWithZeroMoney()
        {
            var expectedMoney = 0m;
            var accountMoney = _account.Money;

            Assert.Equal(expectedMoney, accountMoney);
        }

        [Fact]
        public void IncreaseMoneyGivenAmountIngressed()
        { 
            
            _account.Ingress(100m);
            
            var expectedMoney = 100m;
            var accountMoney = _account.Money;
            Assert.Equal(expectedMoney, accountMoney);
        }

        [Fact]
        public void ThrowExceptionIfNegativeAmmountIsIngressed()
        {
            Assert.Throws<ArgumentException>(() => _account.Ingress(-100m));
        }

    }
}