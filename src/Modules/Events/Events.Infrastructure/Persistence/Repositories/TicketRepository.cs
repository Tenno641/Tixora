using Dapper;
using Events.Application.Common;

namespace Events.Infrastructure.Persistence.Repositories;

public class TicketRepository: ITicketRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    
    public TicketRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }
    
    public async Task<bool> ExistsAsync(Guid id)
    {
        await using var connection = await _dbConnectionFactory.OpenConnectionAsync();

        string sql = """
                     SELECT * FROM events."Tickets" as t
                     WHERE
                         EXISTS(SELECT 1 FROM events."Tickets" as t WHERE t."EventId" = @id);
                     """;

        bool exist = await connection.ExecuteScalarAsync<bool>(sql);

        return exist;
    }
}