using Tixora.Shared.Domain.Common;

namespace Tickets.Domain.Events;

public sealed class EventCanceledDomainEvent(Guid eventId) : DomainEvent
{
    public Guid EventId { get; } = eventId;
}
