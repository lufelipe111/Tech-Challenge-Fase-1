using ContactRegister.Application;
using ContactRegister.Application.Interfaces.Services;
using ContactRegister.Infrastructure;
using ContactRegister.Infrastructure.Cache;
using ContactRegister.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Prometheus;

namespace ContactRegister.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddControllers();
            builder.Services.AddMemoryCache();
            builder.Services.AddTransient<ICacheService, MemCacheService>();
            builder.Services.AddApplication(builder.Configuration);
            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddMemoryCache();

            builder.Services.AddMetrics();
            builder.Services.UseHttpClientMetrics();
            builder.Services.AddHealthChecks();


            var app = builder.Build();

            // Apply migrations on startup
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                dbContext.Database.Migrate();
            }

            app.UseSwagger();
            app.UseSwaggerUI();

            // Configure the HTTP request pipeline.
            app.UseRouting();
            app.UseHttpMetrics(); // Exposes /metrics
            app.UseMetricServer();
            app.UseHttpMetrics();

            app.MapControllers();
            app.MapMetrics();
            app.MapHealthChecks("/health");

            app.Run();
        }
    }
}

