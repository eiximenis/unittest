using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Api.IntegrationTests
{
    public class HealthCheckEndpointTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;
        public HealthCheckEndpointTests(WebApplicationFactory<Bank.Api.Startup> factory)
        {
            _client = factory.CreateDefaultClient();
        }

        [Fact]
        public async Task HealthCheckEndpointShouldReturnHttpOk()
        {
            var response = await _client.GetAsync("/live");
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

    }
}
