using ErrorOr;
using FluentValidation;
using MediatR;
using Tickets.Application.Common;
using Tickets.Domain.Events;
using Tixora.Shared.Domain.Common;

namespace Tickets.Application.Events;

public record RescheduleEventCommand(Guid EventId, DateTime StartsAtUtc, DateTime? EndsAtUtc) : IRequest<ErrorOr<Success>>;

internal sealed class RescheduleEventCommandHandler : IRequestHandler<RescheduleEventCommand, ErrorOr<Success>>
{
    private readonly IEventRepository _eventRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IUnitOfWork _unitOfWork;
    
    public RescheduleEventCommandHandler(IEventRepository eventRepository, IDateTimeProvider dateTimeProvider, IUnitOfWork unitOfWork)
    {
        _eventRepository = eventRepository;
        _dateTimeProvider = dateTimeProvider;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<ErrorOr<Success>> Handle(RescheduleEventCommand request, CancellationToken cancellationToken)
    {
        Event? @event = await _eventRepository.GetAsync(request.EventId, cancellationToken);

        if (@event is null)
            return EventErrors.NotFound(request.EventId);

        if (request.StartsAtUtc < _dateTimeProvider.UtcNow)
            return EventErrors.StartDateInPast;

        @event.Reschedule(request.StartsAtUtc, request.EndsAtUtc);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success;
    }
}

internal sealed class RescheduleEventCommandValidator : AbstractValidator<RescheduleEventCommand>
{
    public RescheduleEventCommandValidator()
    {
        RuleFor(c => c.EventId).NotEmpty();
        RuleFor(c => c.StartsAtUtc).NotEmpty();
        RuleFor(c => c.EndsAtUtc).Must((cmd, endsAt) => endsAt > cmd.StartsAtUtc).When(c => c.EndsAtUtc.HasValue);
    }
}
