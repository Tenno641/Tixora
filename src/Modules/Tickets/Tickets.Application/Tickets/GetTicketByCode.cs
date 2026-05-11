using System.Data.Common;
using Dapper;
using MediatR;
using Tickets.Domain.Tickets;
using ErrorOr;
using Tixora.Shared.Application.Common;

namespace Tickets.Application.Tickets;

public sealed record GetTicketByCodeQuery(string Code) : IRequest<ErrorOr<TicketResponse>>;

internal sealed class GetTicketByCodeQueryHandler : IRequestHandler<GetTicketByCodeQuery, ErrorOr<TicketResponse>>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    
    public GetTicketByCodeQueryHandler(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<ErrorOr<TicketResponse>> Handle(GetTicketByCodeQuery request, CancellationToken cancellationToken)
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
            WHERE t."Code" = @Code
            """;

        TicketResponse? ticket = await connection.QuerySingleOrDefaultAsync<TicketResponse>(sql, request);
        if (ticket is null)
            return TicketErrors.NotFound(request.Code);

        return ticket;
    }
}
