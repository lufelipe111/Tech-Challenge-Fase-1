using ContactRegister.Application.Interfaces.Repositories;
using ContactRegister.Infrastructure.Messaging.Configuration;
using ContactRegister.Infrastructure.Messaging.Service;
using ContactRegister.Infrastructure.Persistence;
using ContactRegister.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace ContactRegister.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        
        services.AddDatabase(configuration);
        services.AddServices();
        services.AddMessaging(configuration);
        
        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IDddRepository, DddRepository>();
        services.AddScoped<IContactRepository, ContactRepository>();
        
        return services;
    }
    
    private static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        
        services.AddDbContextFactory<AppDbContext>(opt =>
        {
            opt.UseSqlServer(connectionString);
        });
    }

    private static void AddMessaging(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RabbitMqConfiguration>(configuration.GetSection("RabbitMq"));

        services.AddHostedService<RabbitMqInitHostedService>();
        services.AddSingleton<IConnection>(sp =>
        {
            var config = sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<RabbitMqConfiguration>>().Value;
            var factory = new ConnectionFactory
            {
                HostName = config.HostName,
                UserName = config.UserName,
                Password = config.Password
            };

            return factory.CreateConnectionAsync().GetAwaiter().GetResult();
        });
    }
}