using Events.Application.Common.Interfaces;
using Events.Domain.Categories;
using Events.Domain.Tickets;

namespace Events.Infrastructure.Persistence;

using Domain.Events;
using Microsoft.EntityFrameworkCore;

public sealed class EventsDbContext: DbContext, IUnitOfWork
{
    public DbSet<Event> Events { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<Category> Categories { get; set; }

    public EventsDbContext(DbContextOptions<EventsDbContext> options): base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema.Events);

        base.OnModelCreating(modelBuilder);
    }
}