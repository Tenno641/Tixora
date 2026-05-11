using System.Data.Common;
using ErrorOr;
using Evently.Modules.Ticketing.Application.Abstractions.Payments;
using Evently.Modules.Ticketing.Domain.Events;
using Evently.Modules.Ticketing.Domain.Payments;
using FluentValidation;
using MediatR;
using Tickets.Application.Common;
using Tickets.Domain.Carts;
using Tickets.Domain.Customers;
using Tickets.Domain.Events;
using Tickets.Domain.Orders;
using Tickets.Domain.Payments;

namespace Tickets.Application.Orders;

public sealed record CreateOrderCommand(Guid CustomerId) : IRequest<ErrorOr<Success>>;

internal sealed class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, ErrorOr<Success>>
{
    private readonly ICartService _cartService;
    private readonly IOrderRepository _orderRepository;
    private readonly ITicketTypeRepository _ticketTypeRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IPaymentService _paymentService;
    private readonly IPaymentRepository _paymentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateOrderCommandHandler(ICartService cartService, IOrderRepository orderRepository, ITicketTypeRepository ticketTypeRepository, ICustomerRepository customerRepository, IPaymentService paymentService, IPaymentRepository paymentRepository, IUnitOfWork unitOfWork)
    {
        _cartService = cartService;
        _orderRepository = orderRepository;
        _ticketTypeRepository = ticketTypeRepository;
        _customerRepository = customerRepository;
        _paymentService = paymentService;
        _paymentRepository = paymentRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<Success>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        await using DbTransaction transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
        
        Customer? customer = await _customerRepository.GetAsync(request.CustomerId, cancellationToken);
        if (customer is null)
            return CustomerErrors.CustomerIsNotFound(request.CustomerId);

        Order order = Order.Create(customer);
        
        Cart cart = await _cartService.GetAsync(customer.Id, cancellationToken);
        if (cart.Items.Count == 0)
            return CartErrors.CartIsEmpty;

        foreach (CartItem cartItem in cart.Items)
        {
            TicketType? ticketType = await _ticketTypeRepository.GetWithLockAsync(cartItem.TicketTypeId, cancellationToken);
            if (ticketType is null)
                return TicketTypeErrors.NotFound(cartItem.TicketTypeId);

            ErrorOr<Success> result = ticketType.UpdateQuantity(cartItem.Quantity);
            if (result.IsError)
                return result.Errors;

            order.AddItem(ticketType, cartItem.Quantity, cartItem.Price, ticketType.Currency);
        }

        _orderRepository.Insert(order);
        
        PaymentResponse paymentResponse = await _paymentService.ChargeAsync(order.TotalPrice, order.Currency);
        
        Payment payment = Payment.Create(order, paymentResponse.TransactionId, paymentResponse.Amount, paymentResponse.Currency);
        
        _paymentRepository.Insert(payment);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        await transaction.CommitAsync(cancellationToken);
        
        await _cartService.ClearAsync(customer.Id, cancellationToken);
        
        return Result.Success;
    }
}

internal sealed class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator() => RuleFor(c => c.CustomerId).NotEmpty();
}