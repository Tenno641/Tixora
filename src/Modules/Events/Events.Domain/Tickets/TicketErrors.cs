using ErrorOr;

namespace Events.Domain.Tickets;

public static class TicketErrors
{
    public static Error TicketNotFound = Error.NotFound("Get.Ticket", "Ticket is not found.");
}