namespace Bank2.FraudTestApi
{
    public class RedisFraudService : IFraudService
    {
        private readonly RedisConnector _connector;
        public RedisFraudService(RedisConnector connector)
        {
            _connector = connector;
        }

        public async Task<bool> CheckFraud(string iban)
        {
            var db = _connector.Multiplexer.GetDatabase();
            var data = await db.StringGetAsync(iban);

            return data.HasValue;
        }

        public async Task DeleteFraud(string iban)
        {
            var db = _connector.Multiplexer.GetDatabase();
            await db.StringGetDeleteAsync(iban);
        }

        public async Task SetFraud(string iban)
        {
            var db = _connector.Multiplexer.GetDatabase();
            await db.StringSetAsync(iban, "FRAUD");
        }
    }
}
