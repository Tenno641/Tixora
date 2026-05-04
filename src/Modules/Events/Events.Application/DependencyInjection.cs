using Events.Application.Common;
using Events.Application.Common.Behaviors;
using Events.Domain.Common;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Events.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssemblyContaining(typeof(DependencyInjection));
            options.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        services.AddValidatorsFromAssemblyContaining(typeof(DependencyInjection));

        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        return services;
    }
}