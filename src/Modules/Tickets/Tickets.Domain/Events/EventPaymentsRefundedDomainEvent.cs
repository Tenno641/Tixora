using Tixora.Shared.Domain.Common;

namespace Tickets.Domain.Events;

public sealed class EventPaymentsRefundedDomainEvent(Guid eventId) : DomainEvent
{
    public Guid EventId { get; init; } = eventId;
}
