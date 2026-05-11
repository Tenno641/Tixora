using ErrorOr;
using Tickets.Domain.Orders;
using Tixora.Shared.Domain.Common;

namespace Tickets.Domain.Payments;

public sealed class Payment : Entity
{
    public Guid OrderId { get; private set; }

    public Guid TransactionId { get; private set; }

    public decimal Amount { get; private set; }

    public string Currency { get; private set; }

    public decimal? AmountRefunded { get; private set; }

    public DateTime CreatedAtUtc { get; private set; }

    public DateTime? RefundedAtUtc { get; private set; }

    public static Payment Create(Order order, Guid transactionId, decimal amount, string currency, Guid? id = null)
    {
        Payment payment = new Payment(
            order.Id,
            transactionId,
            amount,
            currency,
            createdAtUtc: DateTime.UtcNow,
            id: id);

        payment.RaiseDomainEvent(new PaymentCreatedDomainEvent(payment.Id));

        return payment;
    }

    public ErrorOr<Success> Refund(decimal refundAmount)
    {
        if (AmountRefunded == Amount)
            return PaymentErrors.AlreadyRefunded;

        if (AmountRefunded + refundAmount > Amount)
            return PaymentErrors.NotEnoughFunds;

        AmountRefunded += refundAmount;

        if (Amount == AmountRefunded)
            RaiseDomainEvent(new PaymentRefundedDomainEvent(Id, TransactionId, refundAmount));
        else
            RaiseDomainEvent(new PaymentPartiallyRefundedDomainEvent(Id, TransactionId, refundAmount));

        return Result.Success;
    }
    
    private Payment(Guid orderId, Guid transactionId, decimal amount, string currency, DateTime createdAtUtc, decimal? amountRefunded = null, DateTime? refundedAtUtc = null, Guid? id = null) : base(id)
    {
        OrderId = orderId;
        TransactionId = transactionId;
        Amount = amount;
        Currency = currency;
        AmountRefunded = amountRefunded;
        CreatedAtUtc = createdAtUtc;
        RefundedAtUtc = refundedAtUtc;
    }

    private Payment() { }
}
