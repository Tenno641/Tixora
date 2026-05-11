using Dapper;
using Events.Application.Common.Mappings;
using Events.Domain.Tickets;
using MediatR;
using Tixora.Shared.Application.Common;
using ErrorOr;

namespace Events.Application.Tickets;

public record GetTicketsQuery(Guid EventId): IRequest<ErrorOr<List<TicketResponse>>>;

public class GetTickets : IRequestHandler<GetTicketsQuery, ErrorOr<List<TicketResponse>>>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    
    public GetTickets(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }
    
    public async Task<ErrorOr<List<TicketResponse>>> Handle(GetTicketsQuery request, CancellationToken cancellationToken)
    {
        await using var connection = await _dbConnectionFactory.OpenConnectionAsync();

        const string sql = """
                           SELECT *
                           FROM events."TicketTypes" AS e
                           WHERE e."Id" = @EventId;
                           """;

        List<TicketType> tickets = (await connection.QueryAsync<TicketType>(sql, request)).ToList();

        return tickets.Select(t => t.ToResponse()).ToList();
    }
}