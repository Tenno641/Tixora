using Microsoft.Extensions.DependencyInjection;
using Tixora.Shared.Api;
using Users.Infrastructure;

namespace Users.Api;

public static class UsersModule
{
    public static void AddUsersModule(this IServiceCollection services, string databaseConnectionString)
    {
        services.AddInfrastructure(databaseConnectionString);
        services.RegisterEndpoints(AssemblyReference.Assembly);
    }
}