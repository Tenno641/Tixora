using ErrorOr;
using Events.PublicApi;
using FluentValidation;
using MediatR;
using Tickets.Application.Common;
using Tickets.Domain.Carts;
using Users.PublicApi;

namespace Tickets.Application;

public record AddItemToCartCommand(Guid CustomerId, Guid TicketId, int Quantity) : IRequest<ErrorOr<Success>>;
    
public class AddItemToCart: IRequestHandler<AddItemToCartCommand, ErrorOr<Success>>
{
    private readonly ICartService _cartService;
    private readonly IEventsApi _eventsApi;
    private readonly IUsersApi _usersApi;
    
    public AddItemToCart(ICartService cartService, IEventsApi eventsApi, IUsersApi usersApi)
    {
        _cartService = cartService;
        _eventsApi = eventsApi;
        _usersApi = usersApi;
    }
    
    public async Task<ErrorOr<Success>> Handle(AddItemToCartCommand request, CancellationToken cancellationToken)
    {
        TicketResponse? ticket = await _eventsApi.GetTicketAsync(request.TicketId, cancellationToken);
        if (ticket is null)
            return CartErrors.TicketIsNotFound;

        UserResponse? customer = await _usersApi.GetAsync(request.CustomerId, cancellationToken);
        if (customer is null)
            return CartErrors.EventIsNotFound;

        CartItem cartItem = new CartItem
        {
            Quantity = request.Quantity,
            TicketId = ticket.Id,
            Currency = ticket.Currency,
            Price = ticket.Price
        };
            
        await _cartService.InsertCartItemAsync(request.CustomerId, cartItem, cancellationToken: cancellationToken);

        return Result.Success;
    }
}

public class AddItemToCartCommandValidator : AbstractValidator<AddItemToCartCommand>
{
    public AddItemToCartCommandValidator()
    {
        RuleFor(c => c.CustomerId).NotEmpty();
        RuleFor(c => c.TicketId).NotEmpty();
        RuleFor(c => c.Quantity).GreaterThan(0);
    }
}