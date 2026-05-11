using ErrorOr;

namespace Tickets.Domain.Events;

public static class TicketTypeErrors
{
    public static Error NotFound(Guid ticketTypeId) =>
        Error.NotFound("TicketTypes.Get", $"The ticket type with the identifier {ticketTypeId} was not found");

    public static Error NotEnoughQuantity(decimal availableQuantity) =>
        Error.Failure(
            "TicketTypes.UpdateQuantity",
            $"The ticket type has {availableQuantity} quantity available");
}
