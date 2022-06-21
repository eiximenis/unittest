using Bank.FraudApi.Controllers;
using Bank.FraudApi.IntegrationTests.Seeds;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Bank.FraudApi.IntegrationTests
{
    public class FraudControllerTests : IClassFixture<WebApplicationFactory<RedisConfiguration>>
    {
        private readonly FraudIbanSeeder _seeder;
        private readonly HttpClient _client;
        public FraudControllerTests(WebApplicationFactory<RedisConfiguration> factory)
        {
            var multiplexer = factory.Services.GetService<RedisConnector>().Connection;
            _seeder = new FraudIbanSeeder(multiplexer);
            _client = factory.CreateDefaultClient();
        }

        [Theory]
        [InlineData("1234", true)]
        [InlineData("5678", true)]
        [InlineData("9876", false)]
        [InlineData("4321", false)]
        public async Task HttpGetIbanEndpointShouldReturnTrueIfIbanIsInFraudList(string iban, bool fraud)
        {
            await _seeder.Seed();
            var response = await _client.GetAsync($"/frauds/{iban}");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var json = await response.Content.ReadAsStringAsync();

            var data = JsonSerializer.Deserialize<FraudStatusResponse>(json, 
                new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
            data.Fraud.Should().Be(fraud);
        }

    }
}
