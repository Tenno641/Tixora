using Events.Application.Common;
using Events.Domain.Events;

namespace Events.Infrastructure.Persistence.Repositories;

public class EventRepository: IEventsRepository
{
    private readonly EventsDbContext _dbContext;
    
    public EventRepository(EventsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Insert(Event @event)
    {
        _dbContext.Events.Add(@event);
    }
}