using Bank2.FraudTestApi;
using Bank2.FraudTestApi.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Bank2.FraudApi.IntegrationTests
{
    public class FraudControllerTests : IClassFixture<WebApplicationFactory<RedisConfiguration>>, IAsyncLifetime
    {

        private readonly IFraudService _fraudService;
        private readonly HttpClient _client;
        private readonly IServiceScope _scope;
        public FraudControllerTests(WebApplicationFactory<RedisConfiguration> factory)
        {
            _scope = factory.Services.CreateScope();

            _fraudService = scope.ServiceProvider.GetService<IFraudService>();
            _client = factory.CreateDefaultClient();
        }

        public async Task InitializeAsync()
        {
            await _fraudService.SetFraud("1234");
            await _fraudService.SetFraud("5678");
        }

        [Theory]
        [InlineData("1234", true)]
        [InlineData("5678", true)]
        [InlineData("4321", false)]
        // [InlineData(null, false)]
        // [InlineData("", false)]
        public async Task HttpGetEndpointShouldReturnCorrectValue(string iban, bool fraud)
        {
            var url = $"/fraud/{iban}";
            
            var response = await _client.GetAsync(url);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<FraudStatusResponse>(json,
                new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

            result.Fraud.Should().Be(fraud);
                
        }


        public Task DisposeAsync()
        {
            _scope.Dispose();
            return Task.CompletedTask;
        }

    }
}
