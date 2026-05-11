using ErrorOr;
using Evently.Modules.Ticketing.Domain.Events;
using FluentValidation;
using MediatR;
using Tickets.Application.Common;
using Tickets.Domain.Events;

namespace Tickets.Application.Events.CreateEvent;

public sealed record CreateEventCommand(
    Guid EventId,
    string Title,
    string Description,
    string Location,
    DateTime StartsAtUtc,
    DateTime? EndsAtUtc,
    List<CreateEventCommand.TicketTypeRequest> TicketTypes) : IRequest<ErrorOr<Success>>
{
    public sealed record TicketTypeRequest(
        Guid TicketTypeId,
        Guid EventId,
        string Name,
        decimal Price,
        string Currency,
        decimal Quantity);
}

internal sealed class CreateEventCommandHandler : IRequestHandler<CreateEventCommand, ErrorOr<Success>>
{
    private readonly IEventRepository _eventRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITicketTypeRepository _ticketTypeRepository;

    public CreateEventCommandHandler(IEventRepository eventRepository, IUnitOfWork unitOfWork, ITicketTypeRepository ticketTypeRepository)
    {
        _unitOfWork = unitOfWork;
        _ticketTypeRepository = ticketTypeRepository;
        _eventRepository = eventRepository;
    }

    public async Task<ErrorOr<Success>> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        Event @event = Event.Create(
            request.EventId,
            request.Title,
            request.Description,
            request.Location,
            request.StartsAtUtc,
            request.EndsAtUtc);

        _eventRepository.Insert(@event);

        IEnumerable<TicketType> ticketTypes = request.TicketTypes
            .Select(t => TicketType.Create(@event.Id, t.Name, t.Price, t.Currency, t.Quantity, t.TicketTypeId));

        _ticketTypeRepository.InsertRange(ticketTypes);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success;
    }
}

internal sealed class CreateEventCommandValidator : AbstractValidator<CreateEventCommand>
{
    public CreateEventCommandValidator()
    {
        RuleFor(c => c.EventId).NotEmpty();
        RuleFor(c => c.Title).NotEmpty();
        RuleFor(c => c.Description).NotEmpty();
        RuleFor(c => c.Location).NotEmpty();
        RuleFor(c => c.StartsAtUtc).NotEmpty();
        RuleFor(c => c.EndsAtUtc).Must((cmd, endsAt) => endsAt > cmd.StartsAtUtc).When(c => c.EndsAtUtc.HasValue);

        RuleForEach(c => c.TicketTypes)
            .ChildRules(t =>
            {
                t.RuleFor(r => r.EventId).NotEmpty();
                t.RuleFor(r => r.Name).NotEmpty();
                t.RuleFor(r => r.Price).GreaterThan(decimal.Zero);
                t.RuleFor(r => r.Currency).NotEmpty();
                t.RuleFor(r => r.Quantity).GreaterThan(decimal.Zero);
            });
    }
}
