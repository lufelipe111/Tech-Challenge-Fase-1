using ContactRegister.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ContactRegister.Tests.IntegrationTests.Setup
{
    public class ContactRegisterWebApplicationFactory : WebApplicationFactory<Program>
    {

        public IConfiguration Configuration { get; private set; }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Test");

            builder.ConfigureAppConfiguration((context, configuration) =>
            {
                Configuration = configuration.Build();
            });

            builder.ConfigureTestServices(services =>
            {
                using var provider = services.BuildServiceProvider();

                var dbContext = provider.GetService<AppDbContext>();
                dbContext!.Database.Migrate();
            });
        }
    }
}
