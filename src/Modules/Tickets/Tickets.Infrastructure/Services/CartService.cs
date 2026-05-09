using Tickets.Application.Common;
using Tickets.Domain.Carts;
using Tixora.Shared.Application.Common;

namespace Tickets.Infrastructure.Services;

public class CartService: ICartService
{
    private readonly ICacheService _cache;
    private readonly TimeSpan _defaultExpiration = TimeSpan.FromMinutes(30);
    
    public CartService(ICacheService cache)
    {
        _cache = cache;
    }
    
    public async Task<Cart> GetAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        string cacheKey = GenerateCacheKey(customerId);
        Cart? cart = await _cache.GetAsync<Cart>(cacheKey, cancellationToken);
        return cart ?? Cart.Create(customerId);
    }

    public async Task ClearAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        string cacheKey = GenerateCacheKey(customerId);
        Cart defaultCart = Cart.Create(customerId);
        await _cache.SetAsync(cacheKey, defaultCart, _defaultExpiration, cancellationToken);
    }
    
    public async Task InsertCartItemAsync(Guid customerId, CartItem cartItem, TimeSpan? expiration, CancellationToken cancellationToken = default)
    {
        string cacheKey = GenerateCacheKey(customerId);
        
        Cart cart = await GetAsync(customerId, cancellationToken);

        CartItem? existingCartItems = cart.Items.Find(i => i.TicketId == cartItem.TicketId);

        if (existingCartItems is null)
            cart.Items.Add(cartItem);
        else
            existingCartItems.Quantity += cartItem.Quantity;

        await _cache.SetAsync(cacheKey, cart, expiration ?? _defaultExpiration, cancellationToken);
    }
    
    public async Task RemoveCartItemAsync(Guid customerId, Guid ticketId, CancellationToken cancellationToken = default)
    {
        string cacheKey = GenerateCacheKey(customerId);
        
        Cart cart = await GetAsync(customerId, cancellationToken);
        
        CartItem? existingCartItem = cart.Items.Find(i => i.TicketId == ticketId);
        
        if (existingCartItem == null)
            return;
        
        cart.Items.Remove(existingCartItem);
        
        await _cache.SetAsync(cacheKey, cart, _defaultExpiration, cancellationToken);
    }

    private static string GenerateCacheKey(Guid customerId) => $"carts:{customerId}";
}