using Bank3.FraudApi.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Bank3.FraudApi.IntegrationTests
{
    public class FraudControllerMockTests : IClassFixture<WebApplicationFactory<RedisConfiguration>>
    {
        private readonly Mock<IFraudService> _mock;
        private readonly HttpClient _client;
        public FraudControllerMockTests(WebApplicationFactory<RedisConfiguration> factory)
        {
            _mock = new Mock<IFraudService>();
            _mock.Setup(x => x.CheckFraud("9999")).ReturnsAsync(true);
            _mock.Setup(x => x.CheckFraud(It.Is<string>(s => s != "9999"))).ReturnsAsync(false);

            var customFactory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddTransient<IFraudService>(sp => _mock.Object);
                });

            });

            _client = customFactory.CreateDefaultClient();
        }

        [Fact]
        public async Task MockedTest()
        {
            var url = $"/fraud/9999";
            var response = await _client.GetAsync(url);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<FraudStatusResponse>(json,
                new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            result.Fraud.Should().BeTrue();
            _mock.Verify(m => m.CheckFraud("9999"));

        }
    }
}
