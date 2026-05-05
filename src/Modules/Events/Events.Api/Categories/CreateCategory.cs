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

internal static class CreateCategory
{
    public static void AddEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("categories", async (string name, [FromServices] ISender sender) =>
        {
            CreateCategoryCommand command = new CreateCategoryCommand(name);
            
            ErrorOr<Guid> result = await sender.Send(command);

            return result.IsError
                ? result.ToProblemDetails()
                : Results.CreatedAtRoute("GetCategory", new { Id = result.Value }, result.Value);
        })
        .Produces<Guid>(StatusCodes.Status201Created)
        .Produces<ValidationProblemDetails>(StatusCodes.Status400BadRequest)
        .WithTags(Tags.Categories);
    }
}
