using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.FraudApi.IntegrationTests
{
    public class FraudControllerInteractionTests : IClassFixture<WebApplicationFactory<RedisConfiguration>>
    {
        private readonly HttpClient _client;
        private readonly Mock<IFraudService> _mock;
        public FraudControllerInteractionTests(WebApplicationFactory<RedisConfiguration> factory)
        {
            _mock = new Mock<IFraudService>();
            _mock.Setup(x => x.CheckFraud("1234")).ReturnsAsync(true);
            _mock.Setup(x => x.CheckFraud("4321")).ReturnsAsync(false);

            var customFactory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddTransient<IFraudService>(sp =>
                    {           
                        return _mock.Object;
                    });
                });
            });

            _client = customFactory.CreateDefaultClient();
        }

        [Fact]
        public async Task GetEndpointShouldReturnFalseIfIbanNotFound()
        {
            var response = await _client.GetAsync("/frauds/4321");
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            _mock.Verify(x => x.CheckFraud("4321"));
            
        }
    }
}
