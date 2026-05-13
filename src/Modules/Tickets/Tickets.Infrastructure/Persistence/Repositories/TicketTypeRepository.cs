using Microsoft.EntityFrameworkCore;
using Tickets.Application.Common;
using Tickets.Domain.Events;

namespace Tickets.Infrastructure.Persistence.Repositories;

internal sealed class TicketTypeRepository : ITicketTypeRepository
{
    private readonly TicketsDbContext _dbContext;
    
    public TicketTypeRepository(TicketsDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<TicketType?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.TicketTypes.SingleOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task<TicketType?> GetWithLockAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext
            .TicketTypes
            .FromSql(
                $"""
                SELECT 
                    t."Id", 
                    t."EventId", 
                    t."Name", 
                    t."Price", 
                    t."Currency", 
                    t."Quantity", 
                    t."AvailableQuantity"
                FROM tickets."TicketTypes" AS t
                WHERE t."Id" = {id}
                FOR UPDATE NOWAIT
                """)
            .SingleOrDefaultAsync(cancellationToken);
    }

    public void InsertRange(IEnumerable<TicketType> ticketTypes)
    {
        _dbContext.TicketTypes.AddRange(ticketTypes);
    }
}