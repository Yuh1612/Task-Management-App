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

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,

        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppSettings.SecretKey)),

        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddControllers(options =>
{
    options.Filters.Add<HttpResponseExceptionFilter>();
});

builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

builder.Services.AddSingleton<IEventBus, EventBusRabbitMQServices>(sp =>
{
    var subscriptionClientName = "queue_test";
    var logger = sp.GetRequiredService<ILogger<EventBusRabbitMQServices>>();
    var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();
    var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
    var serviceScopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
    var retryCount = 5;

    return new EventBusRabbitMQServices(rabbitMQPersistentConnection, logger, eventBusSubcriptionsManager, serviceScopeFactory, subscriptionClientName, retryCount);
});

builder.Services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();

builder.Services.AddTransient<IIntegrationEventHandler<UserCreatedIntergrationEvent>, UserCreatedIntergrationEventHandler>();

builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.AddMediatR(typeof(CreateUserDomainEvent).GetTypeInfo().Assembly);
builder.Services.AddMediatR(typeof(DeleteListTaskDomainEvent).GetTypeInfo().Assembly);
builder.Services.AddMediatR(typeof(DeleteProjectDomainEvent).GetTypeInfo().Assembly);

builder.Services.AddScoped<ApplicationDbContext>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IListTaskRepository, ListTaskRepository>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<ITodoRepository, TodoRepository>();
builder.Services.AddScoped<IAttachmentRepository, AttachmentRepository>();
builder.Services.AddScoped<ILabelRepository, LabelRepository>();
builder.Services.AddScoped<IHistoryRepository, HistoryRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IJwtHandler, JwtHandler>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<ProjectService>();
builder.Services.AddScoped<TaskService>();

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