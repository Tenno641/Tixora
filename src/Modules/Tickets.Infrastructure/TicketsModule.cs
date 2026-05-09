using Microsoft.Extensions.DependencyInjection;
using Tickets.Application.Common;
using Tickets.Infrastructure.Services;
using Tixora.Shared.Application.Common;
using Tixora.Shared.Infrastructure.Common;

namespace Tickets.Infrastructure;

public static class TicketsModule
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<ICacheService, CacheService>();
        services.AddSingleton<ICartService, CartService>();

        return services;
    }
        
}