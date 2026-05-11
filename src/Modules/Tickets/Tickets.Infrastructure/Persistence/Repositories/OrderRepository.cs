using Microsoft.EntityFrameworkCore;
using Tickets.Application.Common;
using Tickets.Domain.Orders;

namespace Tickets.Infrastructure.Persistence.Repositories;

internal sealed class OrderRepository : IOrderRepository
{
    private readonly TicketsDbContext _dbContext;
    
    public OrderRepository(TicketsDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Order?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Orders
            .Include(o => o.OrderItems)
            .SingleOrDefaultAsync(o => o.Id == id, cancellationToken);
    }

    public void Insert(Order order)
    {
        _dbContext.Orders.Add(order);
    }
}
