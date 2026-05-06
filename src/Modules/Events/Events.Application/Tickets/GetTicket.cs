using Dapper;
using Events.Application.Common;
using Events.Domain.Tickets;
using MediatR;
using ErrorOr;
using Tixora.Shared.Application.Common;

namespace Events.Application.Tickets;

public record GetTicketQuery(Guid TicketId, Guid EventId) : IRequest<ErrorOr<TicketResponse>>;
    
public class GetTicket: IRequestHandler<GetTicketQuery, ErrorOr<TicketResponse>>
{
    private readonly IDbConnectionFactory _dbFactory;
    
    public GetTicket(IDbConnectionFactory dbFactory)
    {
        _dbFactory = dbFactory;
    }

    public async Task<ErrorOr<TicketResponse>> Handle(GetTicketQuery request, CancellationToken cancellationToken)
    {
        await using var connection = await _dbFactory.OpenConnectionAsync();

        string sql = """
                     SELECT * 
                     FROM events."Tickets" as t
                     WHERE t."Id" = @TicketId AND t."EventId" = @EventId;
                     """;
        
        Ticket? ticket = await connection.QuerySingleOrDefaultAsync<Ticket>(sql, request);

        if (ticket is null)
            return TicketErrors.TicketNotFound;

        return new TicketResponse(ticket.Id, ticket.EventId, ticket.Name, ticket.Currency, ticket.Price, ticket.Quantity);
    }
}

public record TicketResponse(Guid Id, Guid EventId, string Name, string Currency, decimal Price, int Quantity);