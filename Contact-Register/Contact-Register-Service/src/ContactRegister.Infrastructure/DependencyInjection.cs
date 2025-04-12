using ContactRegister.Application.Interfaces.Messaging;
using ContactRegister.Infrastructure.Messaging.Configuration;
using ContactRegister.Infrastructure.Messaging.Publisher;
using ContactRegister.Infrastructure.Messaging.Service;
using ContactRegister.Infrastructure.Persistence;
using ContactRegister.Infrastructure.Persistence.Repositories;
using ContactRegister.Shared.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace ContactRegister.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDatabase(configuration);
        services.AddRepositories();
        services.AddMessaging(configuration);
        
        return services;
    }
    
    public static IServiceCollection AddConsumerInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingletonDatabase(configuration);
        services.AddSingletonRepositories();
        
        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IDddRepository, DddRepository>();
        services.AddScoped<IContactRepository, ContactRepository>();
        
        return services;
    }
    
    private static IServiceCollection AddSingletonRepositories(this IServiceCollection services)
    {
        services.AddSingleton<IDddRepository, DddRepository>();
        services.AddSingleton<IContactRepository, ContactRepository>();
        
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
    
    private static void AddSingletonDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
                               ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        
        services.AddDbContext<AppDbContext>(opt =>
        {
            opt.UseSqlServer(connectionString);
        }, ServiceLifetime.Singleton);
    }

    private static void AddMessaging(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RabbitMqConfiguration>(configuration.GetSection("RabbitMq"));

        services.AddHostedService<RabbitMqInitHostedService>();
        services.AddSingleton<IConnection>(sp =>
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
        services.AddSingleton<IPublisher, RabbitMqPublisher>();
    }
}