using ContactRegister.Application.Interfaces.Services;
using ContactRegister.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly.Extensions.Http;
using Polly;

namespace ContactRegister.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IDddService, DddService>();
        services.AddScoped<IContactService, ContactService>();
		services.AddHttpClient("BrasilApi", o =>
		{
			o.BaseAddress = new Uri(configuration["BrasilApi:Url"]!);
		}).SetHandlerLifetime(TimeSpan.FromMinutes(5)).AddPolicyHandler(GetRetryPolicy());
		services.AddScoped<IDddApiService, BrasilApiService>();

		return services;
    }

	private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
	{
		return HttpPolicyExtensions
			.HandleTransientHttpError()
			.WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
	}
}