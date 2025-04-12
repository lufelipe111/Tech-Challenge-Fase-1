using ContactRegister.Api;
using ContactRegister.Tests.IntegrationTests.InMemory.Setup;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ContactRegister.Tests.IntegrationTests.Common
{
    [Collection(nameof(ApiCollectionFixture))]
    public abstract class BaseIntegrationTests : IDisposable
    {
        private IServiceScope? _scoped;
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        protected BaseIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        protected TServiceType InjectServiceInstance<TServiceType>() where TServiceType : class
        {
            _scoped = _factory.Services.CreateScope();

            return _scoped.ServiceProvider.GetRequiredService<TServiceType>();
        }
        
        public HttpClient GetClient()
        {
            return _client;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            _scoped?.Dispose();
        }
    }
}
