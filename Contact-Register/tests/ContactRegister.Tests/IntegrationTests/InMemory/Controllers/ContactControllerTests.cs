using System.Net;
using System.Net.Http.Json;
using ContactRegister.Application.Inputs;
using ContactRegister.Infrastructure.Persistence;
using ContactRegister.Tests.IntegrationTests.Common;
using ContactRegister.Tests.IntegrationTests.InMemory.Setup;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ContactRegister.Tests.IntegrationTests.InMemory.Controllers;

public class ContactControllerTests : BaseIntegrationTests
{
    private const string resource = "Contact";

    public ContactControllerTests(ContactRegisterWebApplicationFactory factory) : base(factory) { }

    [Fact]
    public async Task CreateContact_ShouldReturn_OK()
    {
        // Arrange
        var client = GetClient();
        var request = new ContactInput
        {
            FirstName = "Silvana",
            LastName = "Andreia Lavínia Souza",
            Email = "silvanaandreiasouza@cbb.com.br",
            Address = new AddressInput
            {
                AddressLine1 = "5a Travessa da Batalha n 330, Jordão",
                AddressLine2 = "",
                City = "Recife",
                State = "PE",
                PostalCode = "51260-215"
            },
            HomeNumber = "(81) 2644-3282",
            MobileNumber = "(81) 99682-5038",
            Ddd = 21
        };

        // Act
        var response = await client.PostAsJsonAsync($"{resource}/CreateContact", request);

        // Assert
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task CreateContact_ShouldReturn_BadRequest()
    {
        // Arrange
        var client = GetClient();
        var request = new ContactInput();

        // Act
        var response = await client.PostAsJsonAsync($"{resource}/CreateContact", request);
        
        // Assert
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }


    [Fact]
    public async Task GetContacts_ShouldReturn_ArrayStringNotEmpty()
    {
        // Arrange
        var client = GetClient();
        var dbContext = InjectServiceInstance<AppDbContext>();
        dbContext.Dispose();

        var request = new ContactInput
        {
            FirstName = "Silvana 82653898",
            LastName = "Andreia Lavínia Souza",
            Email = "silvanaandreiasouza@cbb.com.br",
            Address = new AddressInput
            {
                AddressLine1 = "5a Travessa da Batalha n 330, Jordão",
                AddressLine2 = "",
                City = "Recife",
                State = "PE",
                PostalCode = "51260-215"
            },
            HomeNumber = "(81) 2644-3282",
            MobileNumber = "(81) 99682-5038",
            Ddd = 21
        };
        await client.PostAsJsonAsync($"{resource}/CreateContact", request);

        // Act
        var response = await client.GetStringAsync($"{resource}/GetContacts?firstName=Silvana%2082653898&dddCode=0&skip=0&take=50");

        // Assert
        response.Should().NotBe("[]");
    }
}
