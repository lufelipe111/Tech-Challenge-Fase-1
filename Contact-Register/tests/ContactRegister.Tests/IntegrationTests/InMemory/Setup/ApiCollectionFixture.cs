using Xunit;

namespace ContactRegister.Tests.IntegrationTests.InMemory.Setup;

[CollectionDefinition(nameof(ApiCollectionFixture))]
public class ApiCollectionFixture : ICollectionFixture<ContactRegisterWebApplicationFactory>
{
}
