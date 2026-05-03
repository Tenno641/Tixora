namespace Events.Application.Common;

using Domain.Events;

public interface IEventsRepository
{
    Task AddEventAsync(Event @event);
    Task<Event?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}
