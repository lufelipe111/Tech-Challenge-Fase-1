using ContactRegister.Infrastructure.Persistence;
using ContactRegister.Tests.IntegrationTests.Fixtures;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Xunit;

namespace ContactRegister.Tests.IntegrationTests.Factories
{
    [Collection("Database")]
    public class ContactRegisterWebApplicationFactory : WebApplicationFactory<Program>
    {

        private readonly DbFixture _fixture;

        public ContactRegisterWebApplicationFactory(DbFixture fixture)
        {
            _fixture = fixture;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Test");
            builder.ConfigureServices(services =>
            {
            });

            builder.ConfigureAppConfiguration((context, configuration) =>
            {
                configuration.AddInMemoryCollection(new[]
                {
                    new KeyValuePair<string, string>("ConnectionStrings:DefaultConnection", _fixture.ConnectionString),
                });
            });
        }
    }
}
