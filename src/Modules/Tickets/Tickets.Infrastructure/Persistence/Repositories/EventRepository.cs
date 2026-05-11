using Evently.Modules.Ticketing.Domain.Events;
using Microsoft.EntityFrameworkCore;
using Tickets.Application.Common;
using Tickets.Domain.Events;

namespace Tickets.Infrastructure.Persistence.Repositories;

internal sealed class EventRepository : IEventRepository
{
    private readonly TicketsDbContext _dbContext;
    
    public EventRepository(TicketsDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Event?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Events.SingleOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public void Insert(Event @event)
    {
        _dbContext.Events.Add(@event);
    }
}
