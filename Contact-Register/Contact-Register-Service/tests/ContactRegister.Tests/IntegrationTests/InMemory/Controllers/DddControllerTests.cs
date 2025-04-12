using System.Net;
using ContactRegister.Tests.IntegrationTests.Common;
using ContactRegister.Tests.IntegrationTests.InMemory.Setup;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ContactRegister.Tests.IntegrationTests.InMemory.Controllers
{
    public class DddControllerTests : BaseIntegrationTests
    {
        private const string resource = "Ddd";

        public DddControllerTests(ContactRegisterWebApplicationFactory factory) : base(factory) { }

        
    }
}
