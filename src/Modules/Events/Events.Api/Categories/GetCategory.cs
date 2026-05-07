using ErrorOr;
using Events.Application.Categories;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Tixora.Shared.Presentation.Common;
using Tixora.Shared.Presentation.Common.Validation;

namespace Events.Api.Categories;

internal sealed class GetCategory: IEndpoint
{
    public void AddEndpoint(IEndpointRouteBuilder app)
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
