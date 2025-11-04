using Basket.Basket.Dtos;
using System.Security.Claims;

namespace Basket.Basket.Features.CreateBasket;

public record CreateBasketRequest(ShoppingCartDto ShoppingCart); 
public record CreateBasketResponse(Guid Id);

public class CreateBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
       app.MapPost("/basket", async (CreateBasketRequest request, ISender sender, ClaimsPrincipal user) =>
        {
            var userName = user.Identity!.Name;
            var updatedShoppingCart = request.ShoppingCart with { UserName = userName! };

            var command = new CreateBasketCommand(updatedShoppingCart);
            var result = await sender.Send(command);
            var response = result.Adapt<CreateBasketResponse>();
            return Results.Created($"/baskets/{response.Id}", response);
        })
        .WithName("CreateBasket")
        .Produces<CreateBasketResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("CreateBasket")
        .WithDescription("Created Basket")
        .RequireAuthorization();
    }
}
