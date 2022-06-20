namespace Bank2.FraudTestApi
{
    public class RedisFraudService : IFraudService
    {
        public Task<bool> CheckFraud(string iban)
        {
            throw new NotImplementedException();
        }

        public Task DeleteFraud(string iban)
        {
            throw new NotImplementedException();
        }

        public Task SetFraud(string iban)
        {
            throw new NotImplementedException();
        }
    }
}
