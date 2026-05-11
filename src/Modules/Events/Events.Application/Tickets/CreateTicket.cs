using ErrorOr;
using Events.Application.Common;
using Events.Application.Common.Interfaces;
using Events.Domain.Events;
using Events.Domain.Tickets;
using FluentValidation;
using MediatR;

namespace Events.Application.Tickets;

public record CreateTicketCommand(Guid EventId, string Name, string Currency, decimal Price, int Quantity): IRequest<ErrorOr<Guid>>;
        
public class CreateTicket: IRequestHandler<CreateTicketCommand, ErrorOr<Guid>>
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IEventsRepository _eventsRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public CreateTicket(ITicketRepository ticketRepository, IEventsRepository eventsRepository, IUnitOfWork unitOfWork)
    {
        _ticketRepository = ticketRepository;
        _eventsRepository = eventsRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<Guid>> Handle(CreateTicketCommand request, CancellationToken cancellationToken)
    {
        Event? @event = await _eventsRepository.GetByIAsync(request.EventId);
        if (@event == null)
            return EventErrors.EventIsNotFound;

        TicketType ticketType = TicketType.Create(
            @event: @event,
            name: request.Name,
            currency: request.Currency,
            price: request.Price,
            quantity: request.Quantity);

        _ticketRepository.Insert(ticketType);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ticketType.Id;
    }
}

public sealed class CreateTicketCommandValidator : AbstractValidator<CreateTicketCommand>
{
    public CreateTicketCommandValidator()
    {
        RuleFor(e => e.Name).NotEmpty();
        RuleFor(e => e.Currency).NotEmpty();
        RuleFor(e => e.Quantity).GreaterThan(0);
        RuleFor(e => e.Price).GreaterThanOrEqualTo(0);
    }
}