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
using Xunit;

namespace ContactRegister.Tests.IntegrationTests.TestContainers.Factories;

public class TestContainerContactRegisterFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MsSqlContainer _msSqlContainer = new MsSqlBuilder()
        .WithName($"sql-server-test-{Guid.NewGuid()}")
        .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
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
                b.UseSqlServer(connectionString).UseSeeding((context, _) =>
				{
					var ddd = new Ddd(11, "SP", "EMBU, VÁRZEA PAULISTA, VARGEM GRANDE PAULISTA, VARGEM, TUIUTI, TABOÃO DA SERRA, SUZANO, SÃO ROQUE, SÃO PAULO, SÃO LOURENÇO DA SERRA, SÃO CAETANO DO SUL, SÃO BERNARDO DO CAMPO, SANTO ANDRÉ, SANTANA DE PARNAÍBA, SANTA ISABEL, SALTO, SALESÓPOLIS, RIO GRANDE DA SERRA, RIBEIRÃO PIRES, POÁ, PIRAPORA DO BOM JESUS, PIRACAIA, PINHALZINHO, PEDRA BELA, OSASCO, NAZARÉ PAULISTA, MORUNGABA, MOGI DAS CRUZES, MAUÁ, MAIRIPORÃ, MAIRINQUE, JUQUITIBA, JUNDIAÍ, JOANÓPOLIS, JARINU, JANDIRA, ITUPEVA, ITU, ITATIBA, ITAQUAQUECETUBA, ITAPEVI, ITAPECERICA DA SERRA, IGARATÁ, GUARULHOS, GUARAREMA, FRANCO DA ROCHA, FRANCISCO MORATO, FERRAZ DE VASCONCELOS, EMBU-GUAÇU, DIADEMA, COTIA, CARAPICUÍBA, CAMPO LIMPO PAULISTA, CAJAMAR, CAIEIRAS, CABREÚVA, BRAGANÇA PAULISTA, BOM JESUS DOS PERDÕES, BIRITIBA-MIRIM, BARUERI, ATIBAIA, ARUJÁ, ARAÇARIGUAMA, ALUMÍNIO");
					context.Set<Ddd>().Add(ddd);
					context.SaveChanges();

					var contato1 = new Contact("John", "Doe", "john.doe@example.com", new Address("Rua teste, 123", "Predio A, Apartamento 42", "São Paulo", "SP", "012345-678"), new Phone("11111111"), new Phone("922222222"), ddd);
					context.Set<Contact>().Add(contato1);

					var contato2 = new Contact("Jane", "Doe", "jane.doe@example.com", new Address("Rua teste, 123", "Predio A, Apartamento 42", "São Paulo", "SP", "012345-678"), new Phone("11111111"), new Phone("922222222"), ddd);
					context.Set<Contact>().Add(contato2);

					context.SaveChanges();
				});
            });
        });
    }
}