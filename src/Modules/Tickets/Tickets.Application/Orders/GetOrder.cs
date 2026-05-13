using System.Data.Common;
using Dapper;
using ErrorOr;
using Evently.Modules.Ticketing.Domain.Orders;
using MediatR;
using Tickets.Domain.Orders;
using Tixora.Shared.Application.Common;

namespace Tickets.Application.Orders;

public sealed record GetOrderQuery(Guid OrderId) : IRequest<ErrorOr<OrderOrderItemsResponse>>;

internal sealed class GetOrderQueryHandler : IRequestHandler<GetOrderQuery, ErrorOr<OrderOrderItemsResponse>>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    
    public GetOrderQueryHandler(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }
    
    public async Task<ErrorOr<OrderOrderItemsResponse>> Handle(GetOrderQuery request, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await _dbConnectionFactory.OpenConnectionAsync();
        
        const string sql =
            $"""
             SELECT
                 o."Id",
                 o."CustomerId",
                 o."Status",
                 o."TotalPrice",
                 o."CreatedAtUtc",
                 oi."Id" AS {nameof(OrderItemResponse.OrderItemId)},
                 oi."OrderId",
                 oi."TicketTypeId",
                 oi."Quantity",
                 oi."UnitPrice",
                 oi."Price",
                 oi."Currency"
             FROM tickets."Orders" AS o
             JOIN tickets."OrderItems" AS oi ON oi."OrderId" = o."Id"
             WHERE o."Id" = @OrderId
             """;

        Dictionary<Guid, OrderOrderItemsResponse> ordersDictionary = [];
        await connection.QueryAsync<OrderOrderItemsResponse, OrderItemResponse, OrderOrderItemsResponse>(
            sql,
            (order, orderItem) =>
            {
                if (ordersDictionary.TryGetValue(order.Id, out OrderOrderItemsResponse? existing))
                    order = existing;
                else
                    ordersDictionary.Add(order.Id, order);
                
                order.OrderItems.Add(orderItem);
                return order;
            },
            request,
            splitOn: nameof(OrderItemResponse.OrderItemId));

        if (!ordersDictionary.TryGetValue(request.OrderId, out OrderOrderItemsResponse? orderResponse))
            return OrderErrors.NotFound(request.OrderId);

        return orderResponse;
    }
}

public sealed record OrderOrderItemsResponse(Guid Id, Guid CustomerId, OrderStatus Status, decimal TotalPrice, DateTime CreatedAtUtc)
{
    public List<OrderItemResponse> OrderItems { get; } = [];
}

public sealed record OrderItemResponse(
    Guid OrderItemId,
    Guid OrderId,
    Guid TicketTypeId,
    decimal Quantity,
    decimal UnitPrice,
    decimal Price,
    string Currency);