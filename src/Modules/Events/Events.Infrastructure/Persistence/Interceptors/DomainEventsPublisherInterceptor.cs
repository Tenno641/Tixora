using MediatR;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Tixora.Shared.Domain.Common;

namespace Events.Infrastructure.Persistence.Interceptors;

public class DomainEventsPublisherInterceptor: SaveChangesInterceptor
{
    private readonly IPublisher _publisher;

    public DomainEventsPublisherInterceptor(IPublisher publisher)
    {
        _publisher = publisher;
    }

    public async override ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = new CancellationToken())
    {
        List<IDomainEvent> domainEvents = eventData.Context?.ChangeTracker
            .Entries<Entity>()
            .SelectMany(entity => entity.Entity.PopDomainEvents())
            .ToList() ?? [];

        foreach (IDomainEvent domainEvent in domainEvents)
            await _publisher.Publish(domainEvent, cancellationToken);

        return await base.SavedChangesAsync(eventData, result, cancellationToken);
    }
}