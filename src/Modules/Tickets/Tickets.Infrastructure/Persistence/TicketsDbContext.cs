using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Tickets.Application.Common;
using Tickets.Domain.Customers;
using Tickets.Domain.Events;
using Tickets.Domain.Orders;
using Tickets.Domain.Payments;
using Tickets.Domain.Tickets;
using Order = Tickets.Domain.Orders.Order;

namespace Tickets.Infrastructure.Persistence;

public sealed class TicketsDbContext(DbContextOptions<TicketsDbContext> options)
    : DbContext(options), IUnitOfWork
{
    internal DbSet<Customer> Customers { get; set; }
    internal DbSet<Event> Events { get; set; }
    internal DbSet<Ticket> Tickets { get; set; }
    internal DbSet<TicketType> TicketTypes { get; set; }
    internal DbSet<Payment> Payments { get; set; }
    internal DbSet<Order> Orders { get; set; }
    internal DbSet<OrderItem> OrderItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema.Tickets);

        modelBuilder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly);
    }

    public async Task<DbTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (Database.CurrentTransaction is not null)
        {
            await Database.CurrentTransaction.DisposeAsync();
        }

        return (await Database.BeginTransactionAsync(cancellationToken)).GetDbTransaction();
    }
}
