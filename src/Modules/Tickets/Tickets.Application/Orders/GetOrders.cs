using System.Data.Common;
using Dapper;
using Evently.Modules.Ticketing.Domain.Orders;
using MediatR;
using Tixora.Shared.Application.Common;
using ErrorOr;

namespace Tickets.Application.Orders;

public sealed record GetOrdersQuery(Guid CustomerId) : IRequest<ErrorOr<IReadOnlyCollection<OrderResponse>>>;

internal sealed class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, ErrorOr<IReadOnlyCollection<OrderResponse>>>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    
    public GetOrdersQueryHandler(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }
    
    public async Task<ErrorOr<IReadOnlyCollection<OrderResponse>>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await _dbConnectionFactory.OpenConnectionAsync();

        const string sql =
            $"""
             SELECT
                 t."Id",
                 t."CustomerId",
                 t."Status",
                 t."TotalPrice",
                 t."CreatedAtUtc"
             FROM tickets."Orders" AS t
             WHERE t."CustomerId" = @CustomerId
             """;

        List<OrderResponse> orders = (await connection.QueryAsync<OrderResponse>(sql, request)).AsList();
        return orders;
    }
}

public record OrderResponse(Guid Id, Guid CustomerId, OrderStatus Status, decimal TotalPrice, DateTime CreatedAtUtc);
