using System.Data.Common;
using ErrorOr;
using Evently.Modules.Ticketing.Domain.Tickets;
using FluentValidation;
using MediatR;
using Tickets.Application.Common;
using Tickets.Domain.Events;
using Tickets.Domain.Tickets;

namespace Tickets.Application.Tickets;

public sealed record ArchiveTicketsForEventCommand(Guid EventId) : IRequest<ErrorOr<Success>>;

internal sealed class ArchiveTicketsForEventCommandHandler : IRequestHandler<ArchiveTicketsForEventCommand, ErrorOr<Success>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEventRepository _eventRepository;
    private readonly ITicketRepository _ticketRepository;
    
    public ArchiveTicketsForEventCommandHandler(IUnitOfWork unitOfWork, IEventRepository eventRepository, ITicketRepository ticketRepository)
    {
        _unitOfWork = unitOfWork;
        _eventRepository = eventRepository;
        _ticketRepository = ticketRepository;
    }
    public async Task<ErrorOr<Success>> Handle(ArchiveTicketsForEventCommand request, CancellationToken cancellationToken)
    {
        await using DbTransaction transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
        
        Event? @event = await _eventRepository.GetAsync(request.EventId, cancellationToken);
        if (@event is null)
            return EventErrors.NotFound(request.EventId);

        IEnumerable<Ticket> tickets = await _ticketRepository.GetForEventAsync(@event, cancellationToken);
        foreach (Ticket ticket in tickets)
        {
            ticket.Archive();
        }

        @event.TicketsArchived();
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        await transaction.CommitAsync(cancellationToken);
        
        return Result.Success;
    }
}

internal sealed class ArchiveTicketsForEventCommandValidator : AbstractValidator<ArchiveTicketsForEventCommand>
{
    public ArchiveTicketsForEventCommandValidator() => RuleFor(c => c.EventId).NotEmpty();
}