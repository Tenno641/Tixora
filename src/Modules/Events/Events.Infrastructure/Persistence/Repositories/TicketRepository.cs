using Dapper;
using Events.Application.Common;
using Events.Application.Common.Interfaces;
using Events.Domain.Tickets;
using Tixora.Shared.Application.Common;

namespace Events.Infrastructure.Persistence.Repositories;

public class TicketRepository: ITicketRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly EventsDbContext _dbContext;
    
    public TicketRepository(IDbConnectionFactory dbConnectionFactory, EventsDbContext dbContext)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _dbContext = dbContext;
    }
    
    public async Task<bool> ExistsAsync(Guid id)
    {
        await using var connection = await _dbConnectionFactory.OpenConnectionAsync();

        string sql = """
                     SELECT EXISTS(SELECT 1 FROM events."Tickets" as t WHERE t."EventId" = @Id);
                     """;

        bool exist = await connection.ExecuteScalarAsync<bool>(sql, new { Id = id });

        return exist;
    }
    
    public void Insert(Ticket ticket)
    {
        _dbContext.Tickets.Add(ticket);
    }
}