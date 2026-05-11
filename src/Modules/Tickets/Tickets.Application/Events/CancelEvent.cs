using ErrorOr;
using MediatR;
using Tickets.Application.Common;
using Tickets.Domain.Events;

namespace Tickets.Application.Events;

public record CancelEventCommand(Guid EventId) : IRequest<ErrorOr<Success>>;

internal sealed class CancelEventCommandHandler : IRequestHandler<CancelEventCommand, ErrorOr<Success>>
{
    private readonly IEventRepository _eventRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CancelEventCommandHandler(IEventRepository eventRepository, IUnitOfWork unitOfWork)
    {
        _eventRepository = eventRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<Success>> Handle(CancelEventCommand request, CancellationToken cancellationToken)
    {
        Event? @event = await _eventRepository.GetAsync(request.EventId, cancellationToken);

        if (@event is null)
            return EventErrors.NotFound(request.EventId);

        @event.Cancel();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success;
    }
}
