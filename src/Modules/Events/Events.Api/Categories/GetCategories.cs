using Events.Api.Common;
using Events.Application.Categories;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using ErrorOr;
using Events.Api.Common.Validation;

namespace Events.Api.Categories;

internal static class GetCategories
{
    public static void AddEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("categories", async ([FromServices] ISender sender) =>
        {
            ErrorOr<List<CategoryResponse>> result = await sender.Send(new GetCategoriesQuery());
            
            return result.IsError
                ? result.ToProblemDetails()
                : Results.Ok(result.Value);
        })
        .Produces<List<CategoryResponse>>()
        .WithTags(Tags.Categories);
    }
}
