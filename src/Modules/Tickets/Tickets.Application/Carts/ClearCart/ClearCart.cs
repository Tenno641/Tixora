using ErrorOr;
using FluentValidation;
using MediatR;
using Tickets.Application.Common;
using Tickets.Domain.Customers;

namespace Tickets.Application.Carts;

public sealed record ClearCartCommand(Guid CustomerId) : IRequest<ErrorOr<Success>>;

internal sealed class ClearCartCommandHandler : IRequestHandler<ClearCartCommand, ErrorOr<Success>>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly ICartService _cartService;

    public ClearCartCommandHandler(ICustomerRepository customerRepository, ICartService cartService)
    {
        _customerRepository = customerRepository;
        _cartService = cartService;
    }

    public async Task<ErrorOr<Success>> Handle(ClearCartCommand request, CancellationToken cancellationToken)
    {
        Customer? customer = await _customerRepository.GetAsync(request.CustomerId, cancellationToken);

        if (customer is null)
            return CustomerErrors.CustomerIsNotFound(request.CustomerId);

        await _cartService.ClearAsync(customer.Id, cancellationToken);

        return Result.Success;
    }
}

internal sealed class ClearCartCommandValidator : AbstractValidator<ClearCartCommand>
{
    public ClearCartCommandValidator()
    {
        RuleFor(c => c.CustomerId).NotEmpty();
    }
}
