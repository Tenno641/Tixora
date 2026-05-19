using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using StackExchange.Redis;
using Tixora.Shared.Application.Common;
using Tixora.Shared.Application.Common.EventBus;
using Tixora.Shared.Infrastructure.Authentication;
using Tixora.Shared.Infrastructure.Authorization;
using Tixora.Shared.Infrastructure.Bus;
using Tixora.Shared.Infrastructure.Persistence;
using Tixora.Shared.Infrastructure.Services;

namespace Tixora.Shared.Infrastructure;

public static class InfrastructureConfiguration
{
    public static IServiceCollection AddInfrastructureSharedConfiguration(this IServiceCollection services, 
        string databaseConnectionString,
        string redisConnectionString,
        Action<IRegistrationConfigurator>[] registrationConfigurators)
    {
        IConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect(redisConnectionString);
        services.AddStackExchangeRedisCache(options => 
        {
            options.ConnectionMultiplexerFactory = () => Task.FromResult(connectionMultiplexer);
        });
        
        services.AddSingleton<IEventBus, EventBus>();
        services.AddMassTransit(config =>
        {
            foreach (Action<IRegistrationConfigurator> configurator in  registrationConfigurators)
                configurator(config);
            
            config.UsingRabbitMq((context, rabbitConfig) =>
            {
                rabbitConfig.ConfigureEndpoints(context);
            });
        });

        services.AddJwtAuthentication();
        services.AddPermissionAuthorization();
        services.AddTransient<IPermissionService, PermissionService>();
        
        services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();

        services.AddSingleton(new NpgsqlDataSourceBuilder(databaseConnectionString).Build());
        services.AddSingleton<ICacheService, CacheService>();

        return services;
    }
}