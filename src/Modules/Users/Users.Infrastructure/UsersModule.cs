using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Users.Application.Common;
using Users.Infrastructure.Identity;
using Users.Infrastructure.Persistence;
using Users.Infrastructure.Persistence.Interceptors;
using Users.Infrastructure.Persistence.Repositories;

namespace Users.Infrastructure;

public static class UsersModule
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string databaseConnectionString, IConfiguration configuration)
    {
        services.AddPersistence(databaseConnectionString, configuration);

        return services;
    }

    private static void AddPersistence(this IServiceCollection services, string databaseConnectionString, IConfiguration configuration)
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
        
        services.AddTransient<IIdentityProviderService, IdentityProviderService>();
        services.AddTransient<KeyCloakAuthRequestHandler>();

        services.Configure<KeyCloakOptions>(configuration.GetSection("Users:KeyCloak"));
        services.AddHttpClient<KeyCloakClient>((sp, httpClient) =>
        {
            KeyCloakOptions keyCloakOptions = sp.GetRequiredService<IOptions<KeyCloakOptions>>().Value;
            
            httpClient.BaseAddress = new Uri(keyCloakOptions.AdminUrl);
        })
        .AddHttpMessageHandler<KeyCloakAuthRequestHandler>();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<UsersDbContext>());
    }
}