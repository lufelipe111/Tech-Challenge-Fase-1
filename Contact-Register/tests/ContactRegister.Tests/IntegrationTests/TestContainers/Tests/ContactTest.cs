using System.Net;
using System.Net.Http.Json;
using ContactRegister.Application.Inputs;
using ContactRegister.Infrastructure.Persistence;
using ContactRegister.Tests.IntegrationTests.Common;
using ContactRegister.Tests.IntegrationTests.TestContainers.Factories;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ContactRegister.Tests.IntegrationTests.TestContainers.Tests;

public class ContactTest : BaseIntegrationTests, IClassFixture<TestContainerContactRegisterFactory>
{
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
        var response = await client.PostAsync("/Contact/CreateContact", JsonContent.Create(request));
        
        // Assert
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }
    
}