using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Bank2.FraudTestApi
{
    public class RedisConnector
    {
        public ConnectionMultiplexer Multiplexer {get;}
        private ILogger _logger;
        private RedisConfiguration _config;

        public RedisConnector(IOptions<RedisConfiguration> config ,Logger<RedisConnector> logger)
        {
            _logger = logger;
            _config = config.Value;
        }

    }
}
