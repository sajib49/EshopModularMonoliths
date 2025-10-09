using Mapster;

namespace Basket.Basket.Features.GetBasket;

public record GetBasketQuery(string UserName)
    :IQuery<GetBasketResult>;

public record GetBasketResult(ShoppingCartDto ShoppingCart);
internal class GetBasketHandler(BasketDbContext dbContext) 
    : IQueryHandler<GetBasketQuery, GetBasketResult>
{
    public async Task<GetBasketResult> Handle(GetBasketQuery request, CancellationToken cancellationToken)
    {
        var basket = await dbContext.ShoppingCarts
            .AsNoTracking()
            .Include(b => b.Items)
            .SingleOrDefaultAsync(b => b.UserName == request.UserName, cancellationToken);

        if (basket == null)
        {
            throw new BasketNotFoundException(request.UserName);
        }

        var basketDto = basket.Adapt<ShoppingCartDto>();

        return new GetBasketResult(basketDto);
    }
}
