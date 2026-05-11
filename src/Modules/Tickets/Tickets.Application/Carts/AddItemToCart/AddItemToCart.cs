using ErrorOr;
using Events.PublicApi;
using FluentValidation;
using MediatR;
using Tickets.Application.Common;
using Tickets.Domain.Carts;
using Tickets.Domain.Customers;

namespace Tickets.Application.Carts;

public record AddItemToCartCommand(Guid CustomerId, Guid TicketId, int Quantity) : IRequest<ErrorOr<Success>>;

public class AddItemToCartCommandHandler : IRequestHandler<AddItemToCartCommand, ErrorOr<Success>>
{
    private readonly ICartService _cartService;
    private readonly IEventsApi _eventsApi;
    private readonly ICustomerRepository _customerRepository;

    public AddItemToCartCommandHandler(ICartService cartService, IEventsApi eventsApi, ICustomerRepository customerRepository)
    {
        _cartService = cartService;
        _eventsApi = eventsApi;
        _customerRepository = customerRepository;
    }

    public async Task<ErrorOr<Success>> Handle(AddItemToCartCommand request, CancellationToken cancellationToken)
    {
        TicketResponse? ticket = await _eventsApi.GetTicketAsync(request.TicketId, cancellationToken);
        if (ticket is null)
            return CartErrors.TicketIsNotFound;

        Customer? customer = await _customerRepository.GetAsync(request.CustomerId, cancellationToken);
        if (customer is null)
            return CartErrors.CustomerIsNotFound;

        CartItem cartItem = new()
        {
            Quantity = request.Quantity,
            TicketTypeId = ticket.Id,
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
