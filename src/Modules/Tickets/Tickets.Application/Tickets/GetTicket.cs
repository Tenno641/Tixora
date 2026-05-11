using System.Data.Common;
using Dapper;
using MediatR;
using Tickets.Domain.Tickets;
using ErrorOr;
using Tixora.Shared.Application.Common;

namespace Tickets.Application.Tickets;

public sealed record GetTicketQuery(Guid TicketId) : IRequest<ErrorOr<TicketResponse>>;

internal sealed class GetTicketQueryHandler : IRequestHandler<GetTicketQuery, ErrorOr<TicketResponse>>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    
    public GetTicketQueryHandler(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<ErrorOr<TicketResponse>> Handle(GetTicketQuery request, CancellationToken cancellationToken)
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
            WHERE t."Id" = @TicketId
            """;

        TicketResponse? ticket = await connection.QuerySingleOrDefaultAsync<TicketResponse>(sql, request);
        if (ticket is null)
            return TicketErrors.NotFound(request.TicketId);

        return ticket;
    }
}

public sealed record TicketResponse(
    Guid Id,
    Guid CustomerId,
    Guid OrderId,
    Guid EventId,
    Guid TicketTypeId,
    string Code,
    DateTime CreatedAtUtc);
