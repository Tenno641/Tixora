namespace Events.Api.Endpoints;

using Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Persistence;

public static class GetEvent
{
    public static void AddEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("events/{id:guid}", async (Guid id, EventsDbContext dbContext) =>
        {
            var @event = await dbContext.Events
                .AsNoTracking()
                .Where(e => e.Id == id)
                .FirstOrDefaultAsync(e => e.Id == id);

            return @event is null
                ? Results.NotFound()
                : Results.Ok(new Response(
                    Id: @event.Id,
                    Title: @event.Title,
                    Description: @event.Description,
                    Location: @event.Location,
                    StartAt: @event.StartAt,
                    EndAt: @event.EndAt,
                    State: @event.State.ToString()));
        })
        .WithName("GetEvent")
        .WithTags(Tags.Events);
    }
}
