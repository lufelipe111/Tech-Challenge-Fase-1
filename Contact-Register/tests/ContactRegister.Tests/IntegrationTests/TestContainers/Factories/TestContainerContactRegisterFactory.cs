using ContactRegister.Infrastructure.Persistence;
using DotNet.Testcontainers.Builders;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;
using Xunit;

namespace ContactRegister.Tests.IntegrationTests.TestContainers.Factories;

public class TestContainerContactRegisterFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MsSqlContainer _msSqlContainer = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2022-CU17-ubuntu-22.04")
        .WithPortBinding("1433", "1433")
        .WithPassword("Password123")
        .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(1433))
        .Build();
    
    public async Task InitializeAsync()
    {
        await _msSqlContainer.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        await _msSqlContainer.StopAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.ConfigureTestServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
            if (descriptor is not null)
            {
                services.Remove(descriptor);
            }
            services.AddDbContext<AppDbContext>(b =>
            {
                var connectionString = _msSqlContainer.GetConnectionString();
                b.UseSqlServer(connectionString);
            });
        });
    }
}