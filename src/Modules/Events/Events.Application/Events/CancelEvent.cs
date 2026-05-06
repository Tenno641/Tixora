using ErrorOr;
using Events.Application.Common;
using Events.Application.Common.Interfaces;
using Events.Domain.Events;
using MediatR;
using Tixora.Shared.Domain.Common;

namespace Events.Application.Events;

public record CancelEventCommand(Guid Id) : IRequest<ErrorOr<Success>>;

public class CancelEvent: IRequestHandler<CancelEventCommand, ErrorOr<Success>>
{
    private readonly IEventsRepository _eventsRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IUnitOfWork _unitOfWork;
    
    public CancelEvent(IEventsRepository eventsRepository, IDateTimeProvider dateTimeProvider, IUnitOfWork unitOfWork)
    {
        _eventsRepository = eventsRepository;
        _dateTimeProvider = dateTimeProvider;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<ErrorOr<Success>> Handle(CancelEventCommand request, CancellationToken cancellationToken)
    {
        Event? @event = await _eventsRepository.GetByIAsync(request.Id);
        if (@event is null)
            return EventErrors.EventIsNotFound;
        
        ErrorOr<Success> cancellationResult = @event.Cancel(_dateTimeProvider);

        if (cancellationResult.IsError)
            return cancellationResult.Errors;

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return cancellationResult.Value;
    }
}