using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank2.Api.IntegrationTests
{
    public class HealthCheckEndpointsTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;
        public HealthCheckEndpointsTests(WebApplicationFactory<Startup> factory )
        {
            _client = factory.CreateDefaultClient();
        }
        
        [Fact]
        public async Task HealthCheckShouldReturnHttpOk()
        {
            var response = await _client.GetAsync("/live");
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
        
    }
}
