using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Tickets.Application.Carts;
using Tixora.Shared.Api.Common;
using Tixora.Shared.Api.Common.Validation;

namespace Tickets.Api.Carts;

internal sealed class GetCart: IEndpoint
{
    public void AddEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("carts/{customerId:guid}", async (Guid customerId, ISender sender) =>
        {
            GetCartQuery query = new GetCartQuery(customerId);

            ErrorOr<CartResponse> result = await sender.Send(query);
            
            return result.IsError
                ? result.ToProblemDetails()
                : Results.Ok(result.Value);
        })
        .WithTags(Tags.Cart);
    }
}