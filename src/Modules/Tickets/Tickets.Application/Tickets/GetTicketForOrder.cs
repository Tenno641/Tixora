using System.Data.Common;
using Dapper;
using ErrorOr;
using MediatR;
using Tixora.Shared.Application.Common;

namespace Tickets.Application.Tickets;

public sealed record GetTicketsForOrderQuery(Guid OrderId) : IRequest<ErrorOr<IReadOnlyCollection<TicketResponse>>>;

internal sealed class GetTicketsForOrderQueryHandler : IRequestHandler<GetTicketsForOrderQuery, ErrorOr<IReadOnlyCollection<TicketResponse>>>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    
    public GetTicketsForOrderQueryHandler(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }
    
    public async Task<ErrorOr<IReadOnlyCollection<TicketResponse>>> Handle(GetTicketsForOrderQuery request, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await _dbConnectionFactory.OpenConnectionAsync();

        const string sql =
            """
            SELECT
                t."Id",
                t."CustomerId",
                t."OrderId",
                t."EventId",
                t."TicketTypeId",
                t."Code",
                t."CreatedAtUtc"
            FROM tickets."Tickets" AS t
            WHERE t."OrderId" = @OrderId
            """;

        List<TicketResponse> tickets = (await connection.QueryAsync<TicketResponse>(sql, request)).AsList();
        return tickets;
    }
}
