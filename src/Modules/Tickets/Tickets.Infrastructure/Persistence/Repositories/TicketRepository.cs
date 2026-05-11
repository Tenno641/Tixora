using Evently.Modules.Ticketing.Domain.Tickets;
using Microsoft.EntityFrameworkCore;
using Tickets.Domain.Events;
using Tickets.Domain.Tickets;

namespace Tickets.Infrastructure.Persistence.Repositories;

internal sealed class TicketRepository : ITicketRepository
{
    private readonly TicketsDbContext _dbContext;
    
    public TicketRepository(TicketsDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<IEnumerable<Ticket>> GetForEventAsync(
        Event @event,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Tickets.Where(t => t.EventId == @event.Id).ToListAsync(cancellationToken);
    }

    public void InsertRange(IEnumerable<Ticket> tickets)
    {
        _dbContext.Tickets.AddRange(tickets);
    }
}
