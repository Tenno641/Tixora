using Tixora.Shared.Domain.Common;

namespace Tickets.Domain.Tickets;

public sealed class TicketArchivedDomainEvent(Guid ticketId, string code) : DomainEvent
{
    public Guid TicketId { get; init; } = ticketId;

    public string Code { get; init; } = code;
}
