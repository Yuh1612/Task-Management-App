using API;
using API.Authentications;
using API.Extensions;
using API.IntegrationEvents.EventHandling;
using API.IntegrationEvents.Events;
using API.Middleware;
using API.Services;
using Domain.Entities.Projects.Events;
using Domain.Entities.Users.Events;
using Domain.Interfaces;
using Domain.Interfaces.Authentications;
using Domain.Interfaces.Repositories;
using Domain.Models;
using EventBus;
using EventBus.Abstractions;
using EventBusRabbitMQ;
using Infrastructure.Data;
using Infrastructure.Data.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using RabbitMQ.Client;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var configuration = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json", false, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
    .AddEnvironmentVariables().Build();

configuration.GetSection("AppSettings").Get<AppSettings>(options => options.BindNonPublicProperties = true);

var hcBuilder = builder.Services.AddHealthChecks();

hcBuilder.AddRabbitMQ($"amqp://192.168.2.98", name: "rabbitmqbus-test", tags: new string[] { "rabbitmqbus" });

builder.Services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
{
    var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();

    var factory = new ConnectionFactory()
    {
        HostName = "192.168.2.98",
        DispatchConsumersAsync = true,
        UserName = "linh",
        Password = "123456",
    };

    var retryCount = 5;

    return new DefaultRabbitMQPersistentConnection(factory, logger, retryCount);
});

var startup = new Startup(builder.Configuration);

startup.ConfigureServices(builder.Services);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var eventBus = app.Services.GetRequiredService<IEventBus>();
eventBus.Subscribe<UserCreatedIntergrationEvent, IIntegrationEventHandler<UserCreatedIntergrationEvent>>();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseMiddleware<RequestLoggerMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();