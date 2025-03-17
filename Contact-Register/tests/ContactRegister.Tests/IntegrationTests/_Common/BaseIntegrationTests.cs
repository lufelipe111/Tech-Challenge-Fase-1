using ContactRegister.Tests.IntegrationTests.Setup;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ContactRegister.Tests.IntegrationTests._Common
{
    [Collection(nameof(ApiCollectionFixture))]
    public abstract class BaseIntegrationTests : IDisposable
    {
        private IServiceScope _scoped;

        protected ContactRegisterWebApplicationFactory Factory;

        protected HttpClient Client => Factory.CreateClient();

        protected BaseIntegrationTests(ContactRegisterWebApplicationFactory factory)
        {
            Factory = factory;
        }

        protected TServiceType InjectServiceInstance<TServiceType>() where TServiceType : class
        {
            _scoped ??= Factory.Services.CreateScope();

            return _scoped.ServiceProvider.GetRequiredService<TServiceType>();
        }

        public void Dispose()
        {
            _scoped?.Dispose();
        }
    }
}
