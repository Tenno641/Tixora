using Microsoft.Extensions.DependencyInjection;

namespace Tixora.Shared.Infrastructure.Authentication;

public static class AuthenticationExtensions
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services)
    {
        services.AddAuthentication().AddJwtBearer();

        services.ConfigureOptions<AuthenticationJwtOptions>();
            
        return services;
    }
}