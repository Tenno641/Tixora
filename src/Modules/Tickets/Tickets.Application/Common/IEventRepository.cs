using Tickets.Domain.Events;

namespace Tickets.Application.Common;

public interface IEventRepository
{
    Task<Event?> GetAsync(Guid id, CancellationToken cancellationToken = default);

    void Insert(Event @event);
}
