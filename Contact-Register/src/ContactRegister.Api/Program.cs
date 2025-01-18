using ContactRegister.Application;
using ContactRegister.Application.Interfaces.Services;
using ContactRegister.Infrastructure;
using ContactRegister.Infrastructure.Cache;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
	options.SwaggerDoc("v1", new OpenApiInfo
	{
		Version = "v1",
		Title = "API de Registro de Contatos",
		Description = "Possui endpoints para registro, alteração e exclusão de contatos regionais"
	});

	var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
	options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});
builder.Services.AddControllers();
builder.Services.AddMemoryCache();
builder.Services.AddTransient<ICacheService, MemCacheService>();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.UseHttpsRedirection();

await app.RunAsync();