using System.Net;
using System.Net.Http.Json;
using ContactRegister.Application.DTOs;
using ContactRegister.Application.Inputs;
using ContactRegister.Infrastructure.Persistence;
using ContactRegister.Tests.IntegrationTests.Common;
using ContactRegister.Tests.IntegrationTests.TestContainers.Factories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ContactRegister.Tests.IntegrationTests.TestContainers.Tests;

public class ContactTest : BaseIntegrationTests, IClassFixture<TestContainerContactRegisterFactory>
{
    private readonly string resource = "/Contact";
    public ContactTest(TestContainerContactRegisterFactory factory) : base(factory)
    {
        var context = factory.Services.GetRequiredService<AppDbContext>();
        context.Database.Migrate();
    }

    [Fact(DisplayName = "Create simple contact")]
    public async Task Contact_ShouldBe_Created() 
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
        var response = await client.PostAsync($"{resource}/CreateContact", JsonContent.Create(request));
        
        // Assert
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }
    
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
        var response = await client.GetAsync($"{resource}/GetContacts?firstName=Silvana%2082653898&dddCode=0&skip=0&take=50");

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.OK);
		var contactDtos = await response.Content.ReadFromJsonAsync<List<ContactDto>>();
		contactDtos?.Should().NotBeEmpty();
		contactDtos?.Should().Contain(c => c.MobileNumber!.Number == request.MobileNumber && c.Ddd!.Code == request.Ddd);
    }

	[Fact]
	public async Task GetContactShouldReturnContactDto()
	{
		// Arrange
		var client = GetClient();
		int request = 1;

		// Act
		var response = await client.GetAsync(string.Format($"{resource}/GetContact/{0}", request));

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.OK);
		var contactDto = await response.Content.ReadFromJsonAsync<ContactDto>();
		contactDto?.Id.Should().Be(request);
	}

	[Fact]
	public async Task GetContactsByDddCodes_ShouldReturn_List()
	{
		// Arrange
		var client = GetClient();
		int[] request = [68];

		// Act
		var response = await client.PostAsJsonAsync($"{resource}/GetContactsByDddCodes", request);

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.OK);
		var contactDtos = await response.Content.ReadFromJsonAsync<List<ContactDto>>();
		contactDtos?.Should().NotBeEmpty();
		contactDtos?.Should().Contain(c => c.Ddd!.Code == request[0]);
	}

	[Fact]
	public async Task UpdateContact_ShouldReturn_NoContent()
	{
		// Arrange
		var client = GetClient();
		int requestId = 1;
		ContactInput requestBody = new()
		{
			FirstName = "Joana",
			LastName = "Defoe",
			Email = "joana.defoe@example.com",
			Address = new AddressInput
			{
				AddressLine1 = "Rua teste, 123",
				AddressLine2 = "Predio A, Apartamento 42",
				City = "BRASILÉIA",
				State = "AC",
				PostalCode = "012345-678"
			},
			HomeNumber = "11111111",
			MobileNumber = "922222222",
			Ddd = 68
		};

		// Act
		var response = await client.PutAsJsonAsync(string.Format($"{resource}/UpdateContact/{0}", requestId), requestBody);

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.NoContent);
	}

	[Fact]
	public async Task DeleteContact_ShouldReturn_NoContent()
	{
		// Arrange
		var client = GetClient();
		int request = 1;

		// Act
		var response = await client.DeleteAsync(string.Format($"{resource}/DeleteContact/{0}", request));

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.NoContent);
	}
}