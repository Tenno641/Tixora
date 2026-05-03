using ErrorOr;
using Events.Application.Common;
using Events.Domain.Events;
using FluentValidation;
using MediatR;

namespace Events.Application.Events;


public class CreateEvent: IRequestHandler<CreateEventCommand, ErrorOr<Guid>>
{
    private readonly IEventsRepository _eventsRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public CreateEvent(IEventsRepository eventsRepository, IUnitOfWork unitOfWork)
    {
        _eventsRepository = eventsRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<ErrorOr<Guid>> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        Event @event = Event.Create
        ( 
            id: Guid.CreateVersion7(),
            title: request.Title,
            description: request.Description,
            startAt:  request.StartAt,
            endAt: request.EndAt,
            location: request.Location,
            state: EventState.Draft
        );

        _eventsRepository.Insert(@event);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return @event.Id;
    }
}

public record CreateEventCommand(string Title, string Description, string Location, DateTime StartAt, DateTime EndAt): IRequest<ErrorOr<Guid>>;

public class CreateEventCommandValidator : AbstractValidator<CreateEventCommand>
{
    public CreateEventCommandValidator()
    {
        RuleFor(c => c.Title)
            .NotEmpty()
            .MaximumLength(128);
        
        RuleFor(c => c.Description)
            .NotEmpty()
            .MaximumLength(128);
        
        RuleFor(c => c.Location)
            .NotEmpty()
            .MaximumLength(128);
        
        RuleFor(c => c.StartAt).NotEmpty();
        RuleFor(c => c.EndAt).NotEmpty();
        RuleFor(c => c.EndAt)
            .Must((command, endAt) => command.StartAt < endAt)
            .WithMessage("The end time must be before the start time.");
    }
}