using ErrorOr;
using MediatR;
using Tickets.Application.Common;
using Tickets.Domain.Carts;

namespace Tickets.Application.Carts;

public record GetCartQuery(Guid CustomerId) : IRequest<ErrorOr<CartResponse>>;

public class GetCart: IRequestHandler<GetCartQuery, ErrorOr<CartResponse>>
{
    private readonly ICartService _cartService;
    
    public GetCart(ICartService cartService)
    {
        _cartService = cartService;
    }
    
    public async Task<ErrorOr<CartResponse>> Handle(GetCartQuery request, CancellationToken cancellationToken)
    {
        Cart cart = await _cartService.GetAsync(request.CustomerId, cancellationToken);

        List<CartItemResponse> cartItems = cart.Items.Select(item => new CartItemResponse(item.TicketId, item.Quantity, item.Price, item.Currency)).ToList();
        
        Dictionary<string, decimal> total = cart.Items.GroupBy(item => item.Currency).ToDictionary(group => group.Key, group => group.Sum(item => item.Price * item.Quantity));

        return new CartResponse(cart.CustomerId, cartItems, total);
    }
}

public record CartResponse(Guid CustomerId, List<CartItemResponse> Items, Dictionary<string, decimal> Total);

public record CartItemResponse(Guid TicketId, int Quantity, decimal Price, string Currency);

