using ErrorOr;
using Events.Application.Common;
using Events.Application.Common.Interfaces;
using Events.Domain.Events;
using Events.Domain.Tickets;
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

        Ticket ticket = Ticket.Create(
            @event: @event,
            name: request.Name,
            currency: request.Currency,
            price: request.Price,
            quantity: request.Quantity);

        _ticketRepository.Insert(ticket);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ticket.Id;
    }
}