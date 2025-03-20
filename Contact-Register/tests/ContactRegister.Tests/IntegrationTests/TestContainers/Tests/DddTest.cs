using ContactRegister.Application.DTOs;
using ContactRegister.Infrastructure.Persistence;
using ContactRegister.Tests.IntegrationTests.Common;
using ContactRegister.Tests.IntegrationTests.TestContainers.Factories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace ContactRegister.Tests.IntegrationTests.TestContainers.Tests;

public class DddTest : BaseIntegrationTests, IClassFixture<TestContainerContactRegisterFactory>
{
    private readonly string resource = "/Ddd";
    public DddTest(TestContainerContactRegisterFactory factory) : base(factory)
    {
        var context = factory.Services.GetRequiredService<AppDbContext>();
        context.Database.Migrate();
    }
    
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
		var ddd = await response.Content.ReadFromJsonAsync<DddDto>();
		ddd?.Should().NotBeNull();
		ddd?.Code.Should().Be(dddCode);
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

	[Fact]
	public async Task GetDdd_ShouldReturn_List()
	{
		// Arrange
		var client = GetClient();
		var dddCode = 68;

		// Act
		var response = await client.GetAsync($"{resource}/GetDdd");

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.OK);
		var ddds = await response.Content.ReadFromJsonAsync<List<DddDto>>();
		ddds?.Should().NotBeNull();
        ddds?.Should().Contain(ddd => ddd.Code == dddCode);
	}
}