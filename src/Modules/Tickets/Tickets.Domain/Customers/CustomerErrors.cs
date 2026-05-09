using ErrorOr;

namespace Tickets.Domain.Customers;

public static class CustomerErrors
{
    public static Error CustomerIsNotFound(Guid customerId) => Error.NotFound("Get.Customer", $"The customer with the identifier {customerId} was not found");
}
