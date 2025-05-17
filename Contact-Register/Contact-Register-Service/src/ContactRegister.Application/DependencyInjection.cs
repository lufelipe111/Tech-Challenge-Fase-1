using System.Net;
using ContactRegister.Application.Interfaces.Services;
using ContactRegister.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http.Resilience;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;

namespace ContactRegister.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IDddService, DddService>();
        services.AddScoped<IContactService, ContactService>();
		services.AddHttpClient("brasil-api", o =>
			{
				o.BaseAddress = new Uri(configuration["BrasilApi:Url"]!);
			})
			.SetHandlerLifetime(TimeSpan.FromMinutes(5))
			.AddResilienceHandler("circuit-breaker-retry-resilience", builder =>
			{
				var retryStrategyOptions = new RetryStrategyOptions<HttpResponseMessage>
				{
					ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
						.Handle<Exception>()
						.HandleResult(response => !response.IsSuccessStatusCode),
					BackoffType = DelayBackoffType.Exponential,
					UseJitter = true,
					MaxRetryAttempts = 3,
					Delay = TimeSpan.FromSeconds(2),
				};
		
				var circuitBreakerOptions = new CircuitBreakerStrategyOptions<HttpResponseMessage>
				{
					FailureRatio = 0.5,
					SamplingDuration = TimeSpan.FromMinutes(10),
					MinimumThroughput = 8,
					BreakDuration = TimeSpan.FromSeconds(30),
					ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
						.Handle<Exception>()
						.HandleResult(response => response.StatusCode == HttpStatusCode.BadRequest)
						.HandleResult(response => response.StatusCode == HttpStatusCode.NotFound)
						.HandleResult(response => response.StatusCode == HttpStatusCode.TooManyRequests)
						.HandleResult(response => response.StatusCode == HttpStatusCode.InternalServerError)
				};
				
				builder
					.AddRetry(retryStrategyOptions)
					.AddCircuitBreaker(circuitBreakerOptions)
					;
			})
			.SelectPipelineByAuthority()
			;
		
		services.AddScoped<IDddApiService, BrasilApiService>();

		return services;
    }
}