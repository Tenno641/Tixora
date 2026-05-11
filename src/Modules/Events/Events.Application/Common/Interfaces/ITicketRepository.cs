using Events.Domain.Tickets;

namespace Events.Application.Common.Interfaces;

public interface ITicketRepository
{
    Task<bool> ExistsAsync(Guid id);
    void Insert(TicketType ticketType);
}