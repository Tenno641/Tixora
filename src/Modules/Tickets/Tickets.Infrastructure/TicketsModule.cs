using Evently.Modules.Ticketing.Application.Abstractions.Payments;
using Evently.Modules.Ticketing.Domain.Payments;
using Evently.Modules.Ticketing.Infrastructure.Payments;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Tickets.Application.Common;
using Tickets.Infrastructure.IntegrationEvents;
using Tickets.Infrastructure.Persistence;
using Tickets.Infrastructure.Persistence.Repositories;
using Tickets.Infrastructure.Services;
using Tixora.Shared.Application.Common;
using Tixora.Shared.Infrastructure.Common;

namespace Tickets.Infrastructure;

public static class TicketsModule
{
    public static void ConfigureConsumers(IRegistrationConfigurator registrationConfigurator)
    {
        registrationConfigurator.AddConsumer<UserRegisteredIntegrationEventConsumer>();
    }
    
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string databaseConnectionString)
    {
        services.AddSingleton<ICacheService, CacheService>();
        services.AddSingleton<ICartService, CartService>();

        services.AddDbContext<TicketsDbContext>(options =>
        {
            options.UseNpgsql(databaseConnectionString, config =>
            {
                config.MigrationsHistoryTable("Tickets_Migrations_History", Schema.Tickets);
            });
        });
        
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<TicketsDbContext>());
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddScoped<ITicketRepository, TicketRepository>();
        services.AddScoped<ITicketTypeRepository, TicketTypeRepository>();
        
        services.AddScoped<IPaymentService, PaymentService>();

        return services;
    }
}