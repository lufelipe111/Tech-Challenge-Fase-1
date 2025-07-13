using ContactRegister.Api;
using ContactRegister.Domain.Entities;
using ContactRegister.Domain.ValueObjects;
using ContactRegister.Infrastructure.Persistence;
using DotNet.Testcontainers.Builders;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;
using Testcontainers.RabbitMq;
using Xunit;

namespace ContactRegister.Tests.IntegrationTests.TestContainers.Factories;

public class TestContainerContactRegisterFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MsSqlContainer _msSqlContainer = new MsSqlBuilder()
        .WithName($"sql-server-test-{Guid.NewGuid()}")
        .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
        .WithPassword("Password123")
        .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(1433))
        .Build();

    private readonly RabbitMqContainer _rabbitMqContainer = new RabbitMqBuilder()
        .WithImage("rabbitmq:3.12-management")
        .WithName($"rabbitmq-test-{Guid.NewGuid()}")
        .WithUsername("guest")
        .WithPassword("guest")
        .Build();

    public async Task InitializeAsync()
    {
        await _msSqlContainer.StartAsync();
        await _rabbitMqContainer.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        await _msSqlContainer.StopAsync();
        await _rabbitMqContainer.StopAsync();
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
                var connectionString = _msSqlContainer.GetConnectionString() + ";Database=ContactRegisterTest;TrustServerCertificate=True;";
                b.UseSqlServer(connectionString);
            });

            services.Configure<ContactRegister.Infrastructure.Messaging.Configuration.RabbitMqConfiguration>(options =>
            {
                options.HostName = _rabbitMqContainer.Hostname;
                options.Port = _rabbitMqContainer.GetMappedPublicPort(5672);
                options.UserName = "guest";
                options.Password = "guest";
            });

            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<AppDbContext>();
            db.Database.EnsureDeleted();
            db.Database.Migrate();

            var ddd = new Ddd(68, "AC", "PORTO ACRE, XAPURI, TARAUAC, SENA MADUREIRA, SENADOR GUIOMARD, SANTA ROSA DO PURUS, RODRIGUES ALVES, RIO BRANCO, PORTO WALTER, PLCIDO DE CASTRO, MARECHAL THAUMATURGO, MANOEL URBANO, MNCIO LIMA, JORDO, FEIJ, EPITACIOLNDIA, CRUZEiro DO SUL, CAPIXABA, BUJARI, BRASILIA, ASSIS BRASIL, ACRELNDIA");
            var dddEntity = db.Set<Ddd>().Add(ddd);
            db.SaveChanges();

            var contato1 = new Contact("John", "Doe", "john.doe@example.com", new Address("Rua teste, 123", "Predio A, Apartamento 42", "BRASILIA", "AC", "012345-678"), new Phone("11111111"), new Phone("922222222"), dddEntity.Entity);
            db.Set<Contact>().Add(contato1);

            var contato2 = new Contact("Jane", "Doe", "jane.doe@example.com", new Address("Rua teste, 123", "Predio A, Apartamento 42", "BRASILIA", "AC", "012345-678"), new Phone("11111111"), new Phone("922222222"), dddEntity.Entity);
            db.Set<Contact>().Add(contato2);

            db.SaveChanges();
        });
    }
}