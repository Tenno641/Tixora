using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Tickets.Application;
using Tickets.Application.Carts;
using Tixora.Shared.Api.Common;
using Tixora.Shared.Api.Common.Validation;

namespace Tickets.Api.Carts;

public class AddItemToCart: IEndpoint
{
    public void AddEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("carts", async (AddItemToCartRequest request, ISender sender) =>
        {
            AddItemToCartCommand command = new AddItemToCartCommand(request.CustomerId, request.TicketId, request.Quantity);

            ErrorOr<Success> result = await sender.Send(command);

            return result.IsError
                ? result.ToProblemDetails()
                : Results.Ok();
        })
        .WithTags(Tags.Cart);
    }
}

public record  AddItemToCartRequest(Guid CustomerId, Guid TicketId, int Quantity);