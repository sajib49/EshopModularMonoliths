namespace Basket.Basket.Features.GetBasket;

public record GetBasketQuery(string UserName)
    :IQuery<GetBasketResult>;

public record GetBasketResult(ShoppingCartDto ShoppingCart);
internal class GetBasketHandler(IBasketRepository basketRepository) 
    : IQueryHandler<GetBasketQuery, GetBasketResult>
{
    public async Task<GetBasketResult> Handle(GetBasketQuery request, CancellationToken cancellationToken)
    {
        var basket = await basketRepository.GetBasket(request.UserName, true, cancellationToken);
        
        var basketDto = basket.Adapt<ShoppingCartDto>();

        return new GetBasketResult(basketDto);
    }
}
