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
					var ddd = new Ddd(11, "SP", "EMBU, V�RZEA PAULISTA, VARGEM GRANDE PAULISTA, VARGEM, TUIUTI, TABO�O DA SERRA, SUZANO, S�O ROQUE, S�O PAULO, S�O LOUREN�O DA SERRA, S�O CAETANO DO SUL, S�O BERNARDO DO CAMPO, SANTO ANDR�, SANTANA DE PARNA�BA, SANTA ISABEL, SALTO, SALES�POLIS, RIO GRANDE DA SERRA, RIBEIR�O PIRES, PO�, PIRAPORA DO BOM JESUS, PIRACAIA, PINHALZINHO, PEDRA BELA, OSASCO, NAZAR� PAULISTA, MORUNGABA, MOGI DAS CRUZES, MAU�, MAIRIPOR�, MAIRINQUE, JUQUITIBA, JUNDIA�, JOAN�POLIS, JARINU, JANDIRA, ITUPEVA, ITU, ITATIBA, ITAQUAQUECETUBA, ITAPEVI, ITAPECERICA DA SERRA, IGARAT�, GUARULHOS, GUARAREMA, FRANCO DA ROCHA, FRANCISCO MORATO, FERRAZ DE VASCONCELOS, EMBU-GUA�U, DIADEMA, COTIA, CARAPICU�BA, CAMPO LIMPO PAULISTA, CAJAMAR, CAIEIRAS, CABRE�VA, BRAGAN�A PAULISTA, BOM JESUS DOS PERD�ES, BIRITIBA-MIRIM, BARUERI, ATIBAIA, ARUJ�, ARA�ARIGUAMA, ALUM�NIO");
					context.Set<Ddd>().Add(ddd);
					context.SaveChanges();

					var contato1 = new Contact("John", "Doe", "john.doe@example.com", new Address("Rua teste, 123", "Predio A, Apartamento 42", "S�o Paulo", "SP", "012345-678"), new Phone("11111111"), new Phone("922222222"), ddd);
					context.Set<Contact>().Add(contato1);

					var contato2 = new Contact("Jane", "Doe", "jane.doe@example.com", new Address("Rua teste, 123", "Predio A, Apartamento 42", "S�o Paulo", "SP", "012345-678"), new Phone("11111111"), new Phone("922222222"), ddd);
					context.Set<Contact>().Add(contato2);

					context.SaveChanges();
				});
            });
        });
    }
}