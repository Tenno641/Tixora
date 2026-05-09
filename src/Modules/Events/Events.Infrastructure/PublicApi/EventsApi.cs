using Dapper;
using Events.PublicApi;
using Tixora.Shared.Application.Common;
using TicketResponse = Events.PublicApi.TicketResponse;

namespace Events.Infrastructure.PublicApi;

public class EventsApi : IEventsApi
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    
    public EventsApi(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }
    
    public async Task<TicketResponse?> GetTicketAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await using var connection = await _dbConnectionFactory.OpenConnectionAsync();

        const string sql = """
                           SELECT
                               t."Id",
                               t."Price",
                               t."Currency"
                           FROM events."Tickets" AS t
                           WHERE t."Id" = @TicketId;
                           """;

        TicketResponse? ticket = await connection.QuerySingleOrDefaultAsync<TicketResponse?>(sql, new { TicketId = id });
        
        return ticket;
    }
}