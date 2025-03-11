using Xunit;

namespace ContactRegister.Tests.IntegrationTests.Setup;

[CollectionDefinition(nameof(ApiCollectionFixture))]
public class ApiCollectionFixture : ICollectionFixture<ContactRegisterWebApplicationFactory>
{
}
