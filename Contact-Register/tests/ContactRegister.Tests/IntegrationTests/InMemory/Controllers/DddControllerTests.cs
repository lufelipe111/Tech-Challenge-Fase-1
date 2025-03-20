using System.Net;
using ContactRegister.Tests.IntegrationTests.Common;
using ContactRegister.Tests.IntegrationTests.InMemory.Setup;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ContactRegister.Tests.IntegrationTests.InMemory.Controllers
{
    public class DddControllerTests : BaseIntegrationTests
    {
        private const string resource = "Ddd";

        public DddControllerTests(ContactRegisterWebApplicationFactory factory) : base(factory) { }

        [Fact]
        public async Task GetDddByCode_ShouldReturn_Ok()
        {
            // Arrange
            var client = GetClient();
            var dddCode = 11;

            // Act
            var response = await client.GetAsync($"{resource}/GetDdd/{dddCode}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetDddByCode_ShouldReturn_BadRequest()
        {
            // Arrange
            var client = GetClient();
            var dddCode = 999;

            // Act
            var response = await client.GetAsync($"{resource}/GetDdd/{dddCode}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
