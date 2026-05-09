using Tickets.Domain.Carts;

namespace Tickets.Application.Common;

public interface ICartService
{
    Task<Cart> GetAsync(Guid customerId, CancellationToken cancellationToken = default);
    Task ClearAsync(Guid customerId, CancellationToken cancellationToken = default);
    Task InsertCartItemAsync(Guid customerId, CartItem cartItem, TimeSpan? expiration = null, CancellationToken cancellationToken = default);
    Task RemoveCartItemAsync(Guid customerId, Guid ticketId, CancellationToken cancellationToken = default);
}