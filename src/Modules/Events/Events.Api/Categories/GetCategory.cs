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

internal static class GetCategory
{
    public static void AddEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("categories/{id}", async (Guid id, [FromServices] ISender sender) =>
        {
            GetCategoryQuery query = new GetCategoryQuery(id);
            
            ErrorOr<CategoryResponse> result = await sender.Send(query);
            
            return result.IsError
                ? result.ToProblemDetails()
                : Results.Ok(result.Value);
        })
        .Produces<CategoryResponse>()
        .Produces(StatusCodes.Status404NotFound)
        .WithName("GetCategory")
        .WithTags(Tags.Categories);
    }
}
