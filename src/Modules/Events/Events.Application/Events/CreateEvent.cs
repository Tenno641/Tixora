using Events.Application.Common;
using Events.Domain.Events;
using FluentValidation;
using MediatR;

namespace Events.Application.Events;


public class CreateEvent: IRequestHandler<CreateEventCommand, Guid>
{
    private readonly IEventsRepository _eventsRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public CreateEvent(IEventsRepository eventsRepository, IUnitOfWork unitOfWork)
    {
        _eventsRepository = eventsRepository;
        _unitOfWork = unitOfWork;
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
        
        _eventsRepository.Insert(@event);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return @event.Id;
    }
}

public record CreateEventCommand(string Title, string Description, string Location, DateTime StartAt, DateTime EndAt): IRequest<Guid>;

public class CreateEventCommandValidator : AbstractValidator<CreateEventCommand>
{
    public CreateEventCommandValidator()
    {
        RuleFor(c => c.Title).MaximumLength(128);
        RuleFor(c => c.Description).MaximumLength(128);
        RuleFor(c => c.Location).MaximumLength(128);
        RuleFor(c => c.StartAt).NotEmpty();
        RuleFor(c => c.EndAt).NotEmpty();
        RuleFor(c => c.EndAt).Must((command, endAt) => command.StartAt < endAt);
    }
}
