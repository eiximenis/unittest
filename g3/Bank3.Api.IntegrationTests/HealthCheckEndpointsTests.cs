using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Bank3.Api.IntegrationTests
{
    public class HealthCheckEndpointsTests : IClassFixture<WebApplicationFactory<WeatherForecast>>
    {
        private readonly HttpClient _client;
        public HealthCheckEndpointsTests(WebApplicationFactory<WeatherForecast> factory)
        {
            _client = factory.CreateDefaultClient();
        }
        

        [Fact]
        public async Task HealthCheckEndpointShouldReturnHttpOk()
        {
            var response = await _client.GetAsync("/live");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
