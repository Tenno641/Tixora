namespace Events.Api.Endpoints;

using Api;
using Common;
using Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

public static class CreateEvent
{
    public static void AddEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("events", async (Request request, EventsDbContext dbContext) =>
        {
            var @event = new Event
            {
                Id = Guid.CreateVersion7(),
                Title = request.Title,
                Description = request.Description,
                Location = request.Location,
                StartAt = request.StartAt,
                State = EventState.Draft
            };

            dbContext.Events.Add(@event);
            await dbContext.SaveChangesAsync();

            return Results.CreatedAtRoute("GetEvent", new { id = @event.Id }, @event);
        })
        .WithTags(Tags.Events);
    }
}
