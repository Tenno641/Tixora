using Tickets.Domain.Events;
using Tickets.Domain.Tickets;

namespace Tickets.Application.Common;

public interface ITicketRepository
{
    Task<IEnumerable<Ticket>> GetForEventAsync(Event @event, CancellationToken cancellationToken = default);

    void InsertRange(IEnumerable<Ticket> tickets);
}
