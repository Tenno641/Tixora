using MediatR;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Tixora.Shared.Domain.Common;

namespace Users.Infrastructure.Persistence.Interceptors;

public class DomainEventsPublisherInterceptor: SaveChangesInterceptor
{
    private readonly IServiceScopeFactory _scopeFactory;

    public DomainEventsPublisherInterceptor(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async override ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = new CancellationToken())
    {
        using IServiceScope scope = _scopeFactory.CreateScope();
        
        IPublisher publisher = scope.ServiceProvider.GetRequiredService<IPublisher>();
            
        List<IDomainEvent> domainEvents = eventData.Context?.ChangeTracker
            .Entries<Entity>()
            .SelectMany(entity => entity.Entity.PopDomainEvents())
            .ToList() ?? [];

        foreach (IDomainEvent domainEvent in domainEvents)
            await publisher.Publish(domainEvent, cancellationToken);

        return await base.SavedChangesAsync(eventData, result, cancellationToken);
    }
}