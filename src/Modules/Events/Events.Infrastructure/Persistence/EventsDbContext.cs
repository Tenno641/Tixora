namespace Events.Infrastructure.Persistence;

using Domain.Events;
using Microsoft.EntityFrameworkCore;

public sealed class EventsDbContext: DbContext
{
    public DbSet<Event> Events { get; set; }

    public EventsDbContext(DbContextOptions<EventsDbContext> options): base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema.Events);

        base.OnModelCreating(modelBuilder);
    }
};
