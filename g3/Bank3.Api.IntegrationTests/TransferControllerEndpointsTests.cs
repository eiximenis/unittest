using Bank3.Api.Controllers;
using Bank3.Api.Requests;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Bank3.Api.IntegrationTests
{
    public class TransferControllerEndpointsTests : IClassFixture<WebApplicationFactory<TransferController>>
    {
        private readonly HttpClient _client;
        
        public TransferControllerEndpointsTests(WebApplicationFactory<TransferController> factory)
        {
            var mediatrMock = new Mock<IMediator>();

            mediatrMock.Setup(m => m.Send(It.Is<NewTransferRequest>(tr => tr.From == "1234"), 
                It.IsAny<CancellationToken>())).ReturnsAsync(TransferResult.NotEnoughFunds);

            mediatrMock.Setup(m => m.Send(It.Is<NewTransferRequest>(tr => tr.From == "5678"),
                            It.IsAny<CancellationToken>())).ReturnsAsync(TransferResult.BlockedAccount);

            mediatrMock.Setup(m => m.Send(It.Is<NewTransferRequest>(tr => tr.From == "9999"),
                            It.IsAny<CancellationToken>())).ReturnsAsync(TransferResult.Ok);


            var customFactory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddTransient<IMediator>(sp => mediatrMock.Object);
                });
            });

            _client = customFactory.CreateDefaultClient();
        }

        [Fact]
        public async Task GivenNotEnougthFundsErrorThenTransferControllerShouldReturnBadRequest()
        {
            var request = new NewTransferRequest()
            {
                From = "1234",
                To = "5678",
                Amount = 100,
                SendEmail = true,
                Concept = "Test"
            };
            var json = JsonSerializer.Serialize<NewTransferRequest>(request,
                new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            var response = await 
                _client.PostAsync("/transfer", new StringContent(json, Encoding.UTF8, "application/json"));

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }
    }
}
