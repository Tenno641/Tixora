using Tixora.Shared.Domain.Common;

namespace Tickets.Domain.Orders;

public sealed class OrderItem: Entity
{
    public Guid OrderId { get; private set; }

    public Guid TicketTypeId { get; private set; }

    public decimal Quantity { get; private set; }

    public decimal UnitPrice { get; private set; }

    public decimal Price { get; private set; }

    public string Currency { get; private set; }

    internal static OrderItem Create(Guid orderId, Guid ticketTypeId, decimal quantity, decimal unitPrice, string currency)
    {
        OrderItem orderItem = new OrderItem
        (
            orderId,
            ticketTypeId,
            quantity,
            unitPrice,
            quantity * unitPrice,
            currency
        );

        return orderItem;
    }
    
    private OrderItem(Guid orderId, Guid ticketTypeId, decimal quantity, decimal unitPrice, decimal price, string currency, Guid? id = null): base(id)
    {
        OrderId = orderId;
        TicketTypeId = ticketTypeId;
        Quantity = quantity;
        UnitPrice = unitPrice;
        Price = price;
        Currency = currency;
    }

    private OrderItem() { }
}
