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

internal static class UpdateCategory
{
    public static void AddEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("categories/{id:guid}", async (Guid id, string name, [FromServices] ISender sender) =>
        {
            UpdateCategoryCommand command = new UpdateCategoryCommand(id, name);
            
            ErrorOr<Guid> result = await sender.Send(command);

            return result.IsError
                ? result.ToProblemDetails()
                : Results.Ok(result.Value);
        })
        .Produces<Guid>()
        .Produces<ValidationProblemDetails>(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound)
        .WithTags(Tags.Categories);
    }
}
