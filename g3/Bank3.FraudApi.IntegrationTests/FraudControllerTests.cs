using Bank3.FraudApi.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Bank3.FraudApi.IntegrationTests
{
  
    public class FraudControllerTests : IClassFixture<WebApplicationFactory<RedisConfiguration>>, IAsyncLifetime
    {
        private readonly HttpClient _client;
        private readonly IFraudService _svc;
        public FraudControllerTests(WebApplicationFactory<RedisConfiguration> factory)
        {
            _client = factory.CreateDefaultClient();
            _svc = factory.Services.GetService<IFraudService>();

        }

        public async Task InitializeAsync()
        {
            await _svc.SetFraud("1234");
            await _svc.SetFraud("5678");
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }




        [Theory]
        [InlineData("1234", true)]
        [InlineData("5678", true)]
        [InlineData("4321", false)]
        [InlineData("9876", false)]
        public async Task GivenAnIbanThatIsFraudThenHttpGetEndpointShouldReturnTrue(string iban, bool fraud)
        {
            var url = $"/fraud/{iban}";

            var response = await _client.GetAsync(url);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<FraudStatusResponse>(json,
                new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            result.Fraud.Should().Be(fraud);
        }

    }
}
