using ErrorOr;

namespace Tickets.Domain.Tickets;

public static class TicketErrors
{
    public static Error NotFound(Guid ticketId) =>
        Error.NotFound("Tickets.Get", $"The ticket with the identifier {ticketId} was not found");

    public static Error NotFound(string code) =>
        Error.NotFound("Tickets.Get", $"The ticket with the code {code} was not found");
}
