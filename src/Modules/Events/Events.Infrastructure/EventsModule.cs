using Events.Application.Common.Interfaces;
using Events.Infrastructure.Persistence;
using Events.Infrastructure.Persistence.Interceptors;
using Events.Infrastructure.Persistence.Repositories;
using Events.Infrastructure.PublicApi;
using Events.PublicApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Events.Infrastructure;

public static class EventsModule
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string databaseConnectionString)
    {
        services.AddPersistence(databaseConnectionString);

        return services;
    }

    private static IServiceCollection AddPersistence(this IServiceCollection services, string databaseConnectionString)
    {
        services.AddSingleton<DomainEventsPublisherInterceptor>();
        services.AddDbContext<EventsDbContext>((serviceProvider, options) =>
        {
            options.UseNpgsql(databaseConnectionString, postgresOptions =>
            {
                postgresOptions.MigrationsHistoryTable("Events_Migrations_History", Schema.Events);
            });
            options.AddInterceptors(serviceProvider.GetRequiredService<DomainEventsPublisherInterceptor>());
        });

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<EventsDbContext>());

        services.AddScoped<IEventsRepository, EventRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ITicketRepository, TicketRepository>();
        services.AddScoped<IEventsApi, EventsApi>();
        
        return services;
    }
}