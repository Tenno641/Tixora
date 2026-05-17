using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tixora.Shared.Api;
using Users.Infrastructure;

namespace Users.Api;

public static class UsersModule
{
    public static void AddUsersModule(this IServiceCollection services, string databaseConnectionString, IConfiguration configuration)
    {
        services.AddInfrastructure(databaseConnectionString, configuration);
        services.RegisterEndpoints(AssemblyReference.Assembly);
    }
}