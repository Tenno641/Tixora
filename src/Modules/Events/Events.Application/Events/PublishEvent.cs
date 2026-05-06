using ErrorOr;
using Events.Application.Common;
using Events.Application.Common.Interfaces;
using Events.Domain.Events;
using MediatR;

namespace Events.Application.Events;

public record PublishEventCommand(Guid Id) : IRequest<ErrorOr<Success>>;

public class PublishEvent: IRequestHandler<PublishEventCommand, ErrorOr<Success>>
{
    private readonly IEventsRepository _eventsRepository;
    private readonly ITicketRepository _ticketRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PublishEvent(IEventsRepository eventsRepository, ITicketRepository ticketRepository, IUnitOfWork unitOfWork)
    {
        _eventsRepository = eventsRepository;
        _ticketRepository = ticketRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<Success>> Handle(PublishEventCommand request, CancellationToken cancellationToken)
    {
        Event? @event = await _eventsRepository.GetByIAsync(request.Id);
        if (@event is null)
            return EventErrors.EventIsNotFound;

        if (!await _ticketRepository.ExistsAsync(request.Id))
            return EventErrors.TicketsNotFound;

        ErrorOr<Success> publishingEventResult = @event.Publish();

        if (publishingEventResult.IsError)
            return publishingEventResult.Errors;

        _eventsRepository.Update(@event);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return publishingEventResult.Value;
    }
}