using ErrorOr;

namespace Tickets.Domain.Orders;

public static class OrderErrors
{
    public static Error NotFound(Guid orderId) =>
        Error.NotFound("Orders.Get", $"The order with the identifier {orderId} was not found");
    
    public readonly static Error TicketsAlreadyIssues = 
        Error.Conflict("Order.IssueTicket", "The tickets for this order were already issued");
}
