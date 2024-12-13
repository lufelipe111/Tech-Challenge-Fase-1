using ContactRegister.Application.Interfaces.Services;
using ContactRegister.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ContactRegister.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IDddService, DddService>();
        services.AddScoped<IContactService, ContactService>();
        
        return services;
    }
}