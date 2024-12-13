using ContactRegister.Application.Interfaces.Repositories;
using ContactRegister.Application.Interfaces.Services;
using ContactRegister.Infrastructure.Persistence;
using ContactRegister.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ContactRegister.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        
        services.AddDatabase(connectionString);
        services.AddServices();
        
        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IDddRepository, DddRepository>();
        services.AddScoped<IContactRepository, ContactRepository>();
        
        return services;
    }
    
    private static void AddDatabase(this IServiceCollection services, string? connectionString)
    {
        services.AddDbContextFactory<AppDbContext>(opt =>
        {
            opt.UseSqlServer(connectionString);
        });
    }
}