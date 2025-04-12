using System.Net;
using System.Net.Http.Json;
using ContactRegister.Application.Inputs;
using ContactRegister.Infrastructure.Persistence;
using ContactRegister.Tests.IntegrationTests.Common;
using ContactRegister.Tests.IntegrationTests.InMemory.Setup;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ContactRegister.Tests.IntegrationTests.InMemory.Controllers;

public class ContactControllerTests : BaseIntegrationTests
{
    private const string resource = "Contact";

    public ContactControllerTests(ContactRegisterWebApplicationFactory factory) : base(factory) { }

    
}
