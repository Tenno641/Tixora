using ErrorOr;
using Events.Api.Common;
using Events.Api.Common.Validation;
using Events.Application.Categories;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Events.Api.Categories;

internal static class ArchiveCategory
{
    public static void AddEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("categories/{id}/archive", async (Guid id, [FromServices] ISender sender) =>
        {
            ArchiveCategoryCommand command = new ArchiveCategoryCommand(id);
            
            ErrorOr<Success> result = await sender.Send(command);

            return result.IsError
                ? result.ToProblemDetails()
                : Results.NoContent();
        })
        .Produces(StatusCodes.Status409Conflict)
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .Produces<ValidationProblemDetails>(StatusCodes.Status400BadRequest)
        .WithTags(Tags.Categories);
    }
}
