using ErrorOr;

namespace Tickets.Domain.Payments;

public static class PaymentErrors
{
    public static Error NotFound(Guid paymentId) =>
        Error.NotFound("Payments.Get", $"The payment with the identifier {paymentId} was not found");

    public readonly static Error AlreadyRefunded =
        Error.Conflict("Payments.Refund", "The payment was already refunded");

    public readonly static Error NotEnoughFunds =
        Error.Failure("Payments.Refund", "There are not enough funds for a refund");
}
