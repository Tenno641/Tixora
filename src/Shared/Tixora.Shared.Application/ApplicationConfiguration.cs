using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Tixora.Shared.Application.Common;
using Tixora.Shared.Application.Common.Behaviors;
using Tixora.Shared.Domain.Common;

namespace Tixora.Shared.Application;

public static class ApplicationConfiguration
{
    public static IServiceCollection AddSharedApplicationConfiguration(this IServiceCollection services, Assembly[] assemblies)
    {
        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssemblies(assemblies);
            options.AddOpenBehavior(typeof(ValidationBehavior<,>));
            options.AddOpenBehavior(typeof(LoggingBehavior<,>));
        });

        services.AddValidatorsFromAssemblies(assemblies);

        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        return services;
    }
}