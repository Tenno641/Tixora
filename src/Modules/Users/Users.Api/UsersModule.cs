using Microsoft.Extensions.DependencyInjection;
using Tixora.Shared.Api;
using Users.Infrastructure;

namespace Users.Api;

public static class UsersModule
{
    public static void AddUsersModule(this IServiceCollection services, string connectionString)
    {
        services.AddInfrastructure(connectionString);
        services.RegisterEndpoints(AssemblyReference.Assembly);
    }
}