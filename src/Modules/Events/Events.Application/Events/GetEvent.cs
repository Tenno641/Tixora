using Events.Application.Common;
using Events.Domain.Events;
using MediatR;

namespace Events.Application.Events;

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
        return await _eventsRepository.GetByIAsync(query.Id);
    }
}