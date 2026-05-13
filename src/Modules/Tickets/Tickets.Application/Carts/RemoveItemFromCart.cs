using ErrorOr;
using FluentValidation;
using MediatR;
using Tickets.Application.Common;
using Tickets.Domain.Customers;
using Tickets.Domain.Events;

namespace Tickets.Application.Carts;

public sealed record RemoveItemFromCartCommand(Guid CustomerId, Guid TicketTypeId) : IRequest<ErrorOr<Success>>;

internal sealed class RemoveItemFromCartCommandHandler : IRequestHandler<RemoveItemFromCartCommand, ErrorOr<Success>>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly ITicketTypeRepository _ticketTypeRepository;
    private readonly ICartService _cartService;

    public RemoveItemFromCartCommandHandler(ITicketTypeRepository ticketTypeRepository, ICustomerRepository customerRepository, ICartService cartService)
    {
        _ticketTypeRepository = ticketTypeRepository;
        _customerRepository = customerRepository;
        _cartService = cartService;
    }

    public async Task<ErrorOr<Success>> Handle(RemoveItemFromCartCommand request, CancellationToken cancellationToken)
    {
        Customer? customer = await _customerRepository.GetAsync(request.CustomerId, cancellationToken);

        if (customer is null)
            return CustomerErrors.CustomerIsNotFound(request.CustomerId);

        TicketType? ticketType = await _ticketTypeRepository.GetAsync(request.TicketTypeId, cancellationToken);

        if (ticketType is null)
            return TicketTypeErrors.NotFound(request.TicketTypeId);

        await _cartService.RemoveCartItemAsync(customer.Id, ticketType.Id, cancellationToken);

        return Result.Success;
    }
}

internal sealed class RemoveItemFromCartCommandValidator : AbstractValidator<RemoveItemFromCartCommand>
{
    public RemoveItemFromCartCommandValidator()
    {
        RuleFor(c => c.CustomerId).NotEmpty();
        RuleFor(c => c.TicketTypeId).NotEmpty();
    }
}
