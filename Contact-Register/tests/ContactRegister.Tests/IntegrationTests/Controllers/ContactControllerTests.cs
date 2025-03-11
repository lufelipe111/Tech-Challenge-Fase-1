using ContactRegister.Application.Inputs;
using ContactRegister.Tests.IntegrationTests.Factories;
using System.Net.Http.Json;
using System.Net;
using Xunit;

namespace ContactRegister.Tests.IntegrationTests.Controllers;

[Collection("Database")]
public class ContactControllerTests : IClassFixture<ContactRegisterWebApplicationFactory>
{
    private readonly ContactRegisterWebApplicationFactory _factory;

    public ContactControllerTests(ContactRegisterWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task CreateContact_ShouldReturn_OK()
    {
        //Arrange
        var contact = _factory.CreateClient();

        //Act

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

        var response = await contact.PostAsJsonAsync("ContactController", request);

        //Assert
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
