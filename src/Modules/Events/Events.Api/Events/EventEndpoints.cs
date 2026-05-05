using Microsoft.AspNetCore.Routing;

namespace Events.Api.Events;

public static class EventEndpoints
{
    public static void AddEndpoints(IEndpointRouteBuilder app)
    {
        CreateEvent.AddEndpoint(app);
        GetEvent.AddEndpoint(app);
        SearchEvents.AddEndpoint(app);
        CancelEvent.AddEndpoint(app);
        PublishEvent.AddEndpoint(app);
        RescheduleEvent.AddEndpoint(app);
    }
}