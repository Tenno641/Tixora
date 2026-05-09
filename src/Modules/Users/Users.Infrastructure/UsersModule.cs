using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Users.Application.Common;
using Users.Infrastructure.Persistence;
using Users.Infrastructure.Persistence.Interceptors;
using Users.Infrastructure.Persistence.Repositories;
using Users.Infrastructure.PublicApi;
using Users.PublicApi;

namespace Users.Infrastructure;

public static class UsersModule
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string databaseConnectionString)
    {
        services.AddPersistence(databaseConnectionString);

        return services;
    }

    private static void AddPersistence(this IServiceCollection services, string databaseConnectionString)
    {
        services.AddSingleton<DomainEventsPublisherInterceptor>();
        services.AddDbContext<UsersDbContext>((sp, options) =>
        {
            options.UseNpgsql(databaseConnectionString, npgsqlOptions =>
            {
                npgsqlOptions.MigrationsHistoryTable("Users_Migrations_History", Schemas.Users);
            });
            options.AddInterceptors(sp.GetRequiredService<DomainEventsPublisherInterceptor>());
        });

        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<UsersDbContext>());
        services.AddScoped<IUsersApi, UsersApi>();
    }
}