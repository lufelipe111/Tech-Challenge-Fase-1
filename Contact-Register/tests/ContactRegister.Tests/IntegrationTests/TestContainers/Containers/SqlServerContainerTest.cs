using DotNet.Testcontainers.Builders;
using Testcontainers.MsSql;
using Testcontainers.Xunit;
using Xunit.Abstractions;

namespace ContactRegister.Tests.IntegrationTests.TestContainers.Containers;

public sealed partial class SqlServerContainerTest(ITestOutputHelper testOutputHelper)
    : ContainerTest<MsSqlBuilder, MsSqlContainer>(testOutputHelper)
{
    protected override MsSqlBuilder Configure(MsSqlBuilder builder)
    {
        return builder
            .WithImage("mcr.microsoft.com/mssql/server:2022-CU17-ubuntu-22.04")
            .WithPortBinding("1433", "1433")
            .WithPassword("Password123")
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(1433));
    }
}