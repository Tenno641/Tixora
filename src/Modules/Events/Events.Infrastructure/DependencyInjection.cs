using Events.Application.Common.Interfaces;
using Events.Infrastructure.Persistence;
using Events.Infrastructure.Persistence.Interceptors;
using Events.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Events.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services
            .AddPersistence(connectionString)
            .AddRepositories();

        return services;
    }

    private static IServiceCollection AddPersistence(this IServiceCollection services, string connectionString)
    {
        services.AddSingleton<DomainEventsPublisherInterceptor>();
        services.AddDbContext<EventsDbContext>((serviceProvider, options) =>
        {
            // options.UseNpgsql(Environment.GetEnvironmentVariable("DatabaseConnectionString"), postgresOptions =>
            options.UseNpgsql(connectionString, postgresOptions =>
            {
                postgresOptions.MigrationsHistoryTable("Events_Migrations_History", Schema.Events);
            });
            options.AddInterceptors(serviceProvider.GetRequiredService<DomainEventsPublisherInterceptor>());
        });

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<EventsDbContext>());

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IEventsRepository, EventRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ITicketRepository, TicketRepository>();

        return services;
    }
}