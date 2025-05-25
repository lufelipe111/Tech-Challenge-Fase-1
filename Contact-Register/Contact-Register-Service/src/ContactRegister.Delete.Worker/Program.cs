using ContactRegister.Delete.Worker;
using ContactRegister.Delete.Worker.Interfaces;
using ContactRegister.Delete.Worker.Messaging.Consumer;
using ContactRegister.Delete.Worker.Messaging.Service;
using ContactRegister.Infrastructure;
using ContactRegister.Shared.Messaging.Configuration;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.Configure<RabbitMqConfiguration>(builder.Configuration.GetSection("RabbitMq"));

builder.Services.AddLogging();
builder.Services.AddConsumerInfrastructure(builder.Configuration);
builder.Services.AddSingleton<IConsumer, RabbitMqConsumer>();
builder.Services.AddSingleton<IConnection>(sp =>
{
    var config = sp.GetRequiredService<IOptions<RabbitMqConfiguration>>().Value;
    var factory = new ConnectionFactory
    {
        HostName = config.HostName,
        UserName = config.UserName,
        Password = config.Password
    };

    return factory.CreateConnectionAsync().GetAwaiter().GetResult();
});
builder.Services.AddHostedService<RabbitMqInitHostedService>();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
await host.RunAsync();