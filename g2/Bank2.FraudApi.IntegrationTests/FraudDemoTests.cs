using Bank2.FraudTestApi;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank2.FraudApi.IntegrationTests
{
    public class FraudDemoTests : IClassFixture<WebApplicationFactory<RedisConnector>>
    {
        private readonly Mock<IFraudService> _mock;
        private readonly HttpClient _client;
        public FraudDemoTests(WebApplicationFactory<RedisConnector> factory)
        {
            _mock = new Mock<IFraudService>();
            _mock.Setup(x => x.CheckFraud("1234")).ReturnsAsync(true);
            _mock.Setup(x => x.CheckFraud(It.IsAny<string>())).ReturnsAsync(false);

            var customFactory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddTransient<IFraudService>(sp => _mock.Object);
                });
            });

            _client = customFactory.CreateClient();
            
        }

        [Fact]
        public async Task FraudControllerGetShouldCallFraudService()
        {
            var iban = "1234";
            var url = $"/fraud/{iban}";

            var response = await _client.GetAsync(url);

            _mock.Verify(x => x.CheckFraud("1234"));
        }
    }
}
