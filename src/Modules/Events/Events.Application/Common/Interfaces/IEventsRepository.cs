using Events.Domain.Events;

namespace Events.Application.Common.Interfaces;

public interface IEventsRepository
{
    void Insert(Event @event);
    Task<Event?> GetByIAsync(Guid id);
    void Update(Event @event);
}
