using Tixora.Shared.Domain.Common;

namespace Tickets.Domain.Orders;

public sealed class OrderCreatedDomainEvent(Guid orderId) : DomainEvent
{
    public Guid OrderId { get; init; } = orderId;
}
