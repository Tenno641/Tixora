using Events.Application.Common;
using Events.Infrastructure.Persistence;
using Events.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace Events.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services
            .AddPersistence()
            .AddRepositories();

        return services;
    }

    private static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        // string? connectionString = Environment.GetEnvironmentVariable("DatabaseConnectionString");
        string connectionString = "Server=localhost; Port=5432; Username=postgres; Password=password; Database=Tixora;";
            
        services.AddDbContext<EventsDbContext>(options =>
        {
            // options.UseNpgsql(Environment.GetEnvironmentVariable("DatabaseConnectionString"), postgresOptions =>
            options.UseNpgsql(connectionString, postgresOptions =>
            {
                postgresOptions.MigrationsHistoryTable("Events_Migrations_History", Schema.Events);
            });
        });

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<EventsDbContext>());
        services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();

        services.AddSingleton(new NpgsqlDataSourceBuilder(connectionString).Build());

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