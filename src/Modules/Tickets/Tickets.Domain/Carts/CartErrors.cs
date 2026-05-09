using ErrorOr;

namespace Tickets.Domain.Carts;

public static class CartErrors
{
    public static Error TicketIsNotFound = Error.NotFound(code: "Get.Ticket", description: "Ticket is not found");
    public static Error EventIsNotFound = Error.NotFound(code: "Get.Event", description: "Event is not found");
    public static Error CustomerIsNotFound = Error.NotFound(code: "Get.Customer", description: "Customer is not found");
}