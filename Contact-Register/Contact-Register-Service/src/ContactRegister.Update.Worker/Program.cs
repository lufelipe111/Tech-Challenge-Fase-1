using ContactRegister.Infrastructure;
using ContactRegister.Shared.Messaging.Configuration;
using ContactRegister.Update.Worker;
using ContactRegister.Update.Worker.Interfaces;
using ContactRegister.Update.Worker.Messaging.Consumer;
using ContactRegister.Update.Worker.Messaging.Service;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.Configure<RabbitMqConfiguration>(builder.Configuration.GetSection("RabbitMq"));

builder.Services.AddLogging();
builder.Services.AddConsumerInfrastructure(builder.Configuration);
builder.Services.AddScoped<IConsumer, RabbitMqConsumer>();
builder.Services.AddSingleton<IConnection>(sp =>
{
    var config = sp.GetRequiredService<IOptions<RabbitMqConfiguration>>().Value;
    var factory = new ConnectionFactory
    {
        HostName = config.HostName,
        UserName = config.UserName,
        Password = config.Password,
        Port = 5672
    };

    return factory.CreateConnectionAsync().GetAwaiter().GetResult();
});
builder.Services.AddHostedService<RabbitMqInitHostedService>();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
await host.RunAsync();