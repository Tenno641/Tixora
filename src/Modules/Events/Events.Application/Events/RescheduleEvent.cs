using ErrorOr;
using Events.Application.Common;
using Events.Application.Common.Interfaces;
using Events.Domain.Events;
using MediatR;

namespace Events.Application.Events;

public record RescheduleEventCommand(Guid Id, DateTime StartAt, DateTime EndAt) : IRequest<ErrorOr<Success>>;

public class RescheduleEvent: IRequestHandler<RescheduleEventCommand, ErrorOr<Success>>
{
    private readonly IEventsRepository _eventsRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public RescheduleEvent(IEventsRepository eventsRepository, IUnitOfWork unitOfWork)
    {
        _eventsRepository = eventsRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<ErrorOr<Success>> Handle(RescheduleEventCommand request, CancellationToken cancellationToken)
    {
        Event? @event = await _eventsRepository.GetByIAsync(request.Id);
        if (@event is null)
            return EventErrors.EventIsNotFound;
        
        @event.Reschedule(request.StartAt, request.EndAt);
        
        _eventsRepository.Update(@event);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success;
    }
}