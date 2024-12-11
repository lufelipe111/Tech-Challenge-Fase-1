using ContactRegister.Infrastructure.Persistence;
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