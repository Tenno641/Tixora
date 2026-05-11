namespace Tickets.Application;

public sealed record PaymentResponse(Guid TransactionId, decimal Amount, string Currency);
