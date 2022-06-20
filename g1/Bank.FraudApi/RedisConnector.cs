﻿using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Bank.FraudApi
{
    public class RedisConnector
    {
        public ConnectionMultiplexer Connection { get; }
        private readonly ILogger _logger;
        private RedisConfiguration _config;

        public RedisConnector(IOptions<RedisConfiguration> config, ILogger<RedisConnector> logger)
        {
            _logger = logger;
            _config = config.Value;
        }
    }
}
