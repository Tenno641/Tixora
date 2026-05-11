using Dapper;
using ErrorOr;
using Events.Application.Common;
using Events.Domain.Tickets;
using MediatR;
using Tixora.Shared.Application.Common;

namespace Events.Application.Tickets;

public record UpdateTicketPriceCommand(Guid EventId, Guid TicketId, decimal NewPrice): IRequest<ErrorOr<Guid>>;

public class UpdateTicketPrice: IRequestHandler<UpdateTicketPriceCommand, ErrorOr<Guid>>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    
    public UpdateTicketPrice(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }
    
    public async Task<ErrorOr<Guid>> Handle(UpdateTicketPriceCommand request, CancellationToken cancellationToken)
    {
        await using var connection = await _dbConnectionFactory.OpenConnectionAsync();

        const string fetchTicketSql = """
                           SELECT *
                           FROM events."TicketTypes" AS t
                           WHERE t."Id" = @TicketId AND t."EventId" = @EventId;
                           """;

        TicketType? ticket = await connection.QuerySingleOrDefaultAsync(fetchTicketSql, new { TicketId = request.TicketId, EventId = request.EventId });
        if (ticket is null)
            return TicketTypeErrors.TicketNotFound;
        
        const string updateTicketPriceSql = """
                                           UPDATE events."TicketTypes" AS e
                                           SET "Price" = @NewPrice
                                           WHERE e."Id" = @TicketId AND e."EventId" = @EventId;
                                           """;

        int rowsAffected = await connection.ExecuteAsync(updateTicketPriceSql);

        return rowsAffected == 0
            ? ticket.Id
            : TicketTypeErrors.FailedUpdatingTicketPrice;
    }
}