using Evently.Modules.Ticketing.Application.Abstractions.Payments;
using Tickets.Application;

namespace Tickets.Infrastructure.Services;

internal sealed class PaymentService : IPaymentService
{
    public Task<PaymentResponse> ChargeAsync(decimal amount, string currency)
    {
        return Task.FromResult(new PaymentResponse(Guid.NewGuid(), amount, currency));
    }

    public Task RefundAsync(Guid transactionId, decimal amount)
    {
        return Task.CompletedTask;
    }
}
