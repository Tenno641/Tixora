using ErrorOr;
using Evently.Modules.Ticketing.Domain.Events;
using Evently.Modules.Ticketing.Domain.Tickets;
using FluentValidation;
using MediatR;
using Tickets.Application.Common;
using Tickets.Domain.Events;
using Tickets.Domain.Orders;
using Tickets.Domain.Tickets;

namespace Tickets.Application.Tickets;

public sealed record CreateTicketBatchCommand(Guid OrderId) : IRequest<ErrorOr<Success>>;

internal sealed class CreateTicketBatchCommandHandler : IRequestHandler<CreateTicketBatchCommand, ErrorOr<Success>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOrderRepository _orderRepository;
    private readonly ITicketTypeRepository _ticketTypeRepository;
    private readonly ITicketRepository _ticketRepository;
    
    public CreateTicketBatchCommandHandler(IUnitOfWork unitOfWork, IOrderRepository orderRepository, ITicketTypeRepository ticketTypeRepository, ITicketRepository ticketRepository)
    {
        _unitOfWork = unitOfWork;
        _orderRepository = orderRepository;
        _ticketTypeRepository = ticketTypeRepository;
        _ticketRepository = ticketRepository;
    }
    
    public async Task<ErrorOr<Success>> Handle(CreateTicketBatchCommand request, CancellationToken cancellationToken)
    {
        Order? order = await _orderRepository.GetAsync(request.OrderId, cancellationToken);
        if (order is null)
            return OrderErrors.NotFound(request.OrderId);

        ErrorOr<Success> result = order.IssueTickets();
        if (result.IsError) 
            return result.Errors;

        List<Ticket> tickets = [];
        foreach (OrderItem orderItem in order.OrderItems)
        {
            TicketType? ticketType = await _ticketTypeRepository.GetAsync(orderItem.TicketTypeId, cancellationToken);
            if (ticketType is null)
                return TicketTypeErrors.NotFound(orderItem.TicketTypeId);

            for (int i = 0; i < orderItem.Quantity; i++)
                tickets.Add(Ticket.Create(order, ticketType));
        }

        _ticketRepository.InsertRange(tickets);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success;
    }
}

internal sealed class CreateTicketBatchCommandValidator : AbstractValidator<CreateTicketBatchCommand>
{
    public CreateTicketBatchCommandValidator() => RuleFor(c => c.OrderId).NotEmpty();
}