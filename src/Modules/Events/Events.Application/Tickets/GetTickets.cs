using Dapper;
using Events.Application.Common;
using Events.Domain.Tickets;
using MediatR;
using Events.Application.Common.Contracts.Mappings;

namespace Events.Application.Tickets;

public record GetTicketsQuery(Guid EventId): IRequest<List<TicketResponse>>;

public class GetTickets : IRequestHandler<GetTicketsQuery, List<TicketResponse>>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    
    public GetTickets(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }
    
    public async Task<List<TicketResponse>> Handle(GetTicketsQuery request, CancellationToken cancellationToken)
    {
        await using var connection = await _dbConnectionFactory.OpenConnectionAsync();

        const string sql = """
                           SELECT *
                           FROM events."Tickets" AS e
                           WHERE e."Id" = @EventId;
                           """;

        List<Ticket> tickets = (await connection.QueryAsync<Ticket>(sql, request)).ToList();

        return tickets.Select(t => t.ToResponse()).ToList();
    }
}