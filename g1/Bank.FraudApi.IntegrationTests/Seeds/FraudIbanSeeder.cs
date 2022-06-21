using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.FraudApi.IntegrationTests.Seeds
{
    public class FraudIbanSeeder
    {
        private readonly ConnectionMultiplexer _multiplexer; 
        public FraudIbanSeeder(ConnectionMultiplexer multiplexer)
        {
            _multiplexer = multiplexer;
        }

        internal async Task Seed()
        {
            var db = _multiplexer.GetDatabase();
            await db.StringSetAsync("1234", "FRAUD");
            await db.StringSetAsync("5678", "FRAUD");
        }
    }
}
