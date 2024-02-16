using CurrencyService.Interfaces;
using CurrencyService.Services;
using Mcrio.Configuration.Provider.Docker.Secrets;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Додаємо можливість використання DockerSecrets
builder.Configuration.AddDockerSecrets();
builder.Configuration.AddEnvironmentVariables();

// Додаємо сервіси в контейнер залежностей.

builder.Services.AddControllers();

builder.Services.AddScoped<ICurrencyService, NbuService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(sa =>
{
    sa.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Currency Service",
        Contact = new OpenApiContact
        {
            Name = "Korotash Oleksandr"
        },
    });

    string? projectName = Assembly.GetExecutingAssembly().GetName().Name;
    var xmlFile = $"{projectName}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    sa.IncludeXmlComments(xmlPath);
});

builder.Services.AddHttpClient("Client");

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
