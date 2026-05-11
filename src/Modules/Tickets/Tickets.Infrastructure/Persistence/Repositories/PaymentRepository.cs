using Evently.Modules.Ticketing.Domain.Payments;
using Microsoft.EntityFrameworkCore;
using Tickets.Domain.Events;
using Tickets.Domain.Payments;

namespace Tickets.Infrastructure.Persistence.Repositories;

internal sealed class PaymentRepository : IPaymentRepository
{
    private readonly TicketsDbContext _dbContext;
    
    public PaymentRepository(TicketsDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Payment?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Payments.SingleOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Payment>> GetForEventAsync(
        Event @event,
        CancellationToken cancellationToken = default)
    {
        return await (
            from order in _dbContext.Orders
            join payment in _dbContext.Payments on order.Id equals payment.OrderId
            join orderItem in _dbContext.OrderItems on order.Id equals orderItem.OrderId
            join ticketType in _dbContext.TicketTypes on orderItem.TicketTypeId equals ticketType.Id
            where ticketType.EventId == @event.Id
            select payment).ToListAsync(cancellationToken);
    }

    public void Insert(Payment payment)
    {
        _dbContext.Payments.Add(payment);
    }
}
