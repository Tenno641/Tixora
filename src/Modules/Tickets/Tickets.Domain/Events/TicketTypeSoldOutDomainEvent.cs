using Tixora.Shared.Domain.Common;

namespace Tickets.Domain.Events;

public sealed class TicketTypeSoldOutDomainEvent(Guid ticketTypeId) : DomainEvent
{
    public Guid TicketTypeId { get; init; } = ticketTypeId;
}
