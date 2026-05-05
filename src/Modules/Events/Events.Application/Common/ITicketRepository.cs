using Events.Domain.Tickets;

namespace Events.Application.Common;

public interface ITicketRepository
{
    Task<bool> ExistsAsync(Guid id);
    void Insert(Ticket ticket);
}