using Tickets.Domain.Orders;

namespace Tickets.Application.Common;

public interface IOrderRepository
{
    Task<Order?> GetAsync(Guid id, CancellationToken cancellationToken = default);

    void Insert(Order order);
}
