using System.Data.Common;
using ErrorOr;
using Evently.Modules.Ticketing.Domain.Payments;
using FluentValidation;
using MediatR;
using Tickets.Application.Common;
using Tickets.Domain.Events;
using Tickets.Domain.Payments;

namespace Tickets.Application.Payments;

public sealed record RefundPaymentsForEventCommand(Guid EventId) : IRequest<ErrorOr<Success>>;

internal sealed class RefundPaymentsForEventCommandHandler : IRequestHandler<RefundPaymentsForEventCommand, ErrorOr<Success>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEventRepository _eventRepository;
    private readonly IPaymentRepository _paymentRepository;
    
    public RefundPaymentsForEventCommandHandler(IUnitOfWork unitOfWork, IEventRepository eventRepository, IPaymentRepository paymentRepository)
    {
        _unitOfWork = unitOfWork;
        _eventRepository = eventRepository;
        _paymentRepository = paymentRepository;
    }
    
    public async Task<ErrorOr<Success>> Handle(RefundPaymentsForEventCommand request, CancellationToken cancellationToken)
    {
        await using DbTransaction transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
        
        Event? @event = await _eventRepository.GetAsync(request.EventId, cancellationToken);
        if (@event is null)
            return EventErrors.NotFound(request.EventId);

        IEnumerable<Payment> payments = await _paymentRepository.GetForEventAsync(@event, cancellationToken);
        foreach (Payment payment in payments)
        {
            ErrorOr<Success> result = payment.Refund(payment.Amount - (payment.AmountRefunded ?? decimal.Zero));
            if (result.IsError)
                return result.Errors;
        }

        @event.PaymentsRefunded();
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        await transaction.CommitAsync(cancellationToken);
        
        return Result.Success;
    }
}

internal sealed class RefundPaymentsForEventCommandValidator : AbstractValidator<RefundPaymentsForEventCommand>
{
    public RefundPaymentsForEventCommandValidator() => RuleFor(c => c.EventId).NotEmpty();
}
