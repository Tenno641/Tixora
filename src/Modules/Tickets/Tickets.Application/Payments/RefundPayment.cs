using ErrorOr;
using Evently.Modules.Ticketing.Domain.Payments;
using FluentValidation;
using MediatR;
using Tickets.Application.Common;
using Tickets.Domain.Payments;

namespace Tickets.Application.Payments;

public sealed record RefundPaymentCommand(Guid PaymentId, decimal Amount) : IRequest<ErrorOr<Success>>;

internal sealed class RefundPaymentCommandHandler : IRequestHandler<RefundPaymentCommand, ErrorOr<Success>>
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public RefundPaymentCommandHandler(IPaymentRepository paymentRepository, IUnitOfWork unitOfWork)
    {
        _paymentRepository = paymentRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<ErrorOr<Success>> Handle(RefundPaymentCommand request, CancellationToken cancellationToken)
    {
        Payment? payment = await _paymentRepository.GetAsync(request.PaymentId, cancellationToken);
        if (payment is null)
            return PaymentErrors.NotFound(request.PaymentId);

        ErrorOr<Success> result = payment.Refund(request.Amount);
        if (result.IsError)
            return result.Errors;

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success;
    }
}

internal sealed class RefundPaymentCommandValidator : AbstractValidator<RefundPaymentCommand>
{
    public RefundPaymentCommandValidator() => RuleFor(c => c.PaymentId).NotEmpty();
}