using Events.Application.Common;
using Events.Domain.Events;
using Microsoft.EntityFrameworkCore;

namespace Events.Infrastructure.Persistence.Repositories;

public class EventRepository: IEventsRepository
{
    private readonly EventsDbContext _dbContext;
    
    public EventRepository(EventsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddEventAsync(Event @event)
    {
        _dbContext.Events.Add(@event);
        
        await _dbContext.SaveChangesAsync();
    }
    
    public async Task<Event?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext.Events.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }
}