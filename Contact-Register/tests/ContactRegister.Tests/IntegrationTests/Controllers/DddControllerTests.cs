using Xunit;
using ContactRegister.Tests.IntegrationTests.Setup;
using FluentAssertions;
using ContactRegister.Tests.IntegrationTests._Common;
using System.Net.Http.Json;
using System.Net;

namespace ContactRegister.Tests.IntegrationTests.Controllers
{
    public class DddControllerTests : BaseIntegrationTests
    {
        private const string CommonUri = "Ddd";

        public DddControllerTests(ContactRegisterWebApplicationFactory factory) : base(factory) { }

        [Fact]
        public async Task GetDddByCode_ShouldReturn_Ok()
        {
            // Arrange
            var dddCode = 11;

            // Act
            var response = await Client.GetAsync($"{CommonUri}/GetDdd/{dddCode}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetDddByCode_ShouldReturn_BadRequest()
        {
            // Arrange
            var dddCode = 999;

            // Act
            var response = await Client.GetAsync($"{CommonUri}/GetDdd/{dddCode}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
