using API.Authentications;
using API.Extensions;
using API.IntegrationEvents.EventHandling;
using API.IntegrationEvents.Events;
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
using System.Reflection;
using System.Text;

namespace API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.Filters.Add<HttpResponseExceptionFilter>();
            });

            services.AddControllers();
            services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddHttpContextAccessor();
            services.AddScoped<ApplicationDbContext>();
            services.AddScoped<IJwtHandler, JwtHandler>();

            ConfigureAuthService(services);

            RegisterEventBus(services);

            RegisterMediator(services);

            RegisterInfrastructure(services);

            RegisterServices(services);
        }

        private void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<ProjectService>();
            services.AddScoped<TaskService>();
        }

        private void RegisterInfrastructure(IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IListTaskRepository, ListTaskRepository>();
            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<ITodoRepository, TodoRepository>();
            services.AddScoped<IAttachmentRepository, AttachmentRepository>();
            services.AddScoped<ILabelRepository, LabelRepository>();
            services.AddScoped<IHistoryRepository, HistoryRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        private void RegisterMediator(IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddMediatR(typeof(CreateUserDomainEvent).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(DeleteListTaskDomainEvent).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(DeleteProjectDomainEvent).GetTypeInfo().Assembly);
        }

        private void ConfigureAuthService(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(o =>
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
        }

        private void RegisterEventBus(IServiceCollection services)
        {
            services.AddSingleton<IEventBus, EventBusRabbitMQServices>(sp =>
            {
                var subscriptionClientName = "queue_test";
                var logger = sp.GetRequiredService<ILogger<EventBusRabbitMQServices>>();
                var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();
                var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
                var serviceScopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
                var retryCount = 5;

                return new EventBusRabbitMQServices(rabbitMQPersistentConnection, logger, eventBusSubcriptionsManager, serviceScopeFactory, subscriptionClientName, retryCount);
            });

            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();

            services.AddTransient<IIntegrationEventHandler<UserCreatedIntergrationEvent>, UserCreatedIntergrationEventHandler>();
        }
    }
}