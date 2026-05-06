using Dapper;
using Events.Application.Common;
using Events.Domain.Events;
using MediatR;
using ErrorOr;
using Microsoft.Extensions.Logging;

namespace Events.Application.Events;

public record GetEventQuery(Guid Id): IRequest<ErrorOr<EventTicketResponse>>;

public class GetEvent: IRequestHandler<GetEventQuery, ErrorOr<EventTicketResponse>>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly ILogger<GetEvent> _logger;

    public GetEvent(IDbConnectionFactory dbConnectionFactory, ILogger<GetEvent> logger)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _logger = logger;
    }

    public async Task<ErrorOr<EventTicketResponse>> Handle(GetEventQuery query, CancellationToken cancellationToken)
    {
        await using var connection = await _dbConnectionFactory.OpenConnectionAsync();

        const string sql = """
                           SELECT
                               e."Id",
                               e."Title",
                               e."Description",
                               e."Location",
                               e."CategoryId",
                               e."StartAt",
                               e."EndAt",
                               t."Id" AS "TicketId",
                               t."Name",
                               t."Price",
                               t."Currency",
                               t."Quantity"
                           FROM events."Events" as e
                           LEFT JOIN events."Tickets" as t ON e."Id" = t."EventId"
                           WHERE e."Id" = @Id
                           """;

        Dictionary<Guid, EventTicketResponse> eventsDictionary = [];
        await connection.QueryAsync<EventTicketResponse, EventTicket?, EventTicketResponse>(
            sql,
            (@event, ticketType) =>
            {
                if (eventsDictionary.TryGetValue(@event.Id, out EventTicketResponse? existingEvent))
                {
                    @event = existingEvent;
                }
                else
                {
                    eventsDictionary.Add(@event.Id, @event);
                }

                if (ticketType is not null)
                {
                    @event.Tickets.Add(ticketType);
                }

                return @event;
            },
            query,
            splitOn: nameof(EventTicket.TicketId));

        if (!eventsDictionary.TryGetValue(query.Id, out EventTicketResponse? eventResponse))
            return EventErrors.EventIsNotFound;

        return eventResponse;
        
    }
}

// Dapper is so fragile with records!
public class EventTicketResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Location { get; set; }
    public Guid CategoryId { get; set; }
    public DateTime StartAt { get; set; }
    public DateTime? EndAt { get; set; }
    public List<EventTicket> Tickets { get; } = [];
}

public class EventTicket
{
    public Guid TicketId { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Currency { get; set; }
    public decimal Quantity { get; set; }
}