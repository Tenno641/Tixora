using ErrorOr;
using FluentValidation;
using MediatR;
using Tickets.Application.Common;
using Tickets.Domain.Events;

namespace Tickets.Application.TicketTypes;

public sealed record UpdateTicketTypePriceCommand(Guid TicketTypeId, decimal Price) : IRequest<ErrorOr<Success>>;

internal sealed class UpdateTicketTypePriceCommandHandler : IRequestHandler<UpdateTicketTypePriceCommand, ErrorOr<Success>>
{
    private readonly ITicketTypeRepository _ticketTypeRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public UpdateTicketTypePriceCommandHandler(ITicketTypeRepository ticketTypeRepository, IUnitOfWork unitOfWork)
    {
        _ticketTypeRepository = ticketTypeRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<ErrorOr<Success>> Handle(UpdateTicketTypePriceCommand request, CancellationToken cancellationToken)
    {
        TicketType? ticketType = await _ticketTypeRepository.GetAsync(request.TicketTypeId, cancellationToken);
        if (ticketType is null)
            return TicketTypeErrors.NotFound(request.TicketTypeId);

        ticketType.UpdatePrice(request.Price);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success;
    }
}

internal sealed class UpdateTicketTypePriceCommandValidator : AbstractValidator<UpdateTicketTypePriceCommand>
{
    public UpdateTicketTypePriceCommandValidator()
    {
        RuleFor(c => c.TicketTypeId).NotEmpty();
        RuleFor(c => c.Price).GreaterThan(decimal.Zero);
    }
}