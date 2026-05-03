namespace Events.Application.Events;

using Common;
using Domain.Events;
using MediatR;

public record GetEventQuery(Guid Id): IRequest<Event?>;

public class GetEvent: IRequestHandler<GetEventQuery, Event?>
{
    private readonly IEventsRepository _eventsRepository;

    public GetEvent(IEventsRepository eventsRepository)
    {
        _eventsRepository = eventsRepository;
    }

    public async Task<Event?> Handle(GetEventQuery query, CancellationToken cancellationToken)
    {
        var @event = await _eventsRepository.GetByIdAsync(query.Id, cancellationToken);

        return @event;
    }
}
