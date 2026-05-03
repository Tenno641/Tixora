using Events.Application.Common;
using Events.Domain.Events;
using MediatR;

namespace Events.Application.Events;

public record CreateEventCommand(string Title, string Description, string Location, DateTime StartAt, DateTime EndAt): IRequest<Guid>;

public class CreateEvent: IRequestHandler<CreateEventCommand, Guid>
{
    private readonly IEventsRepository _eventsRepository;
    
    public CreateEvent(IEventsRepository eventsRepository)
    {
        _eventsRepository = eventsRepository;
    }
    
    public async Task<Guid> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        Event @event = new Event
        {
            Id = Guid.CreateVersion7(),
            Title = request.Title,
            Description = request.Description,
            Location = request.Location,
            StartAt = request.StartAt,
            EndAt = request.EndAt,
            State = EventState.Draft
        };
        
        await _eventsRepository.AddEventAsync(@event);
        
        return @event.Id;
    }
}
