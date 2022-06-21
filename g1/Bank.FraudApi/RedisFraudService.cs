using StackExchange.Redis;

namespace Bank.FraudApi
{
    public class RedisFraudService : IFraudService
    {
        private readonly ConnectionMultiplexer _multiplexer;
        public RedisFraudService(RedisConnector connector)
        {
            _multiplexer = connector.Connection;
        }

        public async Task<bool> CheckFraud(string iban)
        {
            var db = _multiplexer.GetDatabase();
            var data = await db.StringGetAsync(iban);

            return data.HasValue;
        }

        public async Task DeleteFraud(string iban)
        {
            var db = _multiplexer.GetDatabase();
            await db.StringGetDeleteAsync(iban);
        }

        public async Task SetFraud(string iban)
        {
            var db = _multiplexer.GetDatabase();
            await db.StringSetAsync(iban, "FRAUD");
        }
    }
}
