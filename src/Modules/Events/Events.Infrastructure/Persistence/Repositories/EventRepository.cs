using System.Data.Common;
using Dapper;
using Events.Application.Common;
using Events.Domain.Events;

namespace Events.Infrastructure.Persistence.Repositories;

public class EventRepository: IEventsRepository
{
    private readonly EventsDbContext _dbContext;
    private readonly IDbConnectionFactory _dbConnectionFactory;
    
    public EventRepository(EventsDbContext dbContext, IDbConnectionFactory dbConnectionFactory)
    {
        _dbContext = dbContext;
        _dbConnectionFactory = dbConnectionFactory;
    }

    public void Insert(Event @event)
    {
        _dbContext.Events.Add(@event);
    }
    
    public async Task<Event?> GetByIAsync(Guid id)
    {
        await using DbConnection connection = await _dbConnectionFactory.OpenConnectionAsync();

        const string sql = """
                           SELECT 
                           e."Id",
                           e."Title",
                           e."Description",
                           e."StartAt",
                           e."EndAt",
                           e."Location",
                           e."State"
                           FROM events."Events" as e
                           WHERE e."Id" = @Id
                           """;

        Event? @event = await connection.QuerySingleOrDefaultAsync<Event>(sql, new { Id = id});
        
        return @event;
    }
}