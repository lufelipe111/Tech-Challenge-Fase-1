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