using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Bank3.FraudApi
{
    public class RedisConnector
    {
        public ConnectionMultiplexer Multiplexer { get; }

        public RedisConnector(IOptions<RedisConfiguration> config)
        {
            var endpoint = config.Value.Host 
                ?? throw new ArgumentException("Redis:Host not set");

            Multiplexer = ConnectionMultiplexer.Connect(new ConfigurationOptions()
            {
                EndPoints = { endpoint }
            });
        }
    }
}
