using System.Data.Common;
using Events.Application.Common;
using Events.Domain.Events;
using MediatR;
using Dapper;

namespace Events.Application.Events;

public record GetEventQuery(Guid Id): IRequest<Event?>;

public class GetEvent: IRequestHandler<GetEventQuery, Event?>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public GetEvent(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<Event?> Handle(GetEventQuery query, CancellationToken cancellationToken)
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

        Event? @event = await connection.QuerySingleOrDefaultAsync<Event>(sql, query);
        
        return @event;
    }
}