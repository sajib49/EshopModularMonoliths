
namespace Basket.Basket.Features.UpdateItemPriceInBasket;


public record UpdateItemPriceInBasketCommand( Guid ProductId, decimal Price)
    : ICommand<UpdateItemPriceInBasketResult>;

public record UpdateItemPriceInBasketResult(bool IsSuccess);

public class UpdateItemPriceInBasketCommandValidator
    : AbstractValidator<UpdateItemPriceInBasketCommand>
{
    public UpdateItemPriceInBasketCommandValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty().WithMessage("ProductId is required.");
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("NewPrice must be greater than zero.");
    }
}

public class UpdateItemPriceInBasketHandler(BasketDbContext dbContext)
    : ICommandHandler<UpdateItemPriceInBasketCommand, UpdateItemPriceInBasketResult>
{
    public async Task<UpdateItemPriceInBasketResult> Handle(UpdateItemPriceInBasketCommand command, CancellationToken cancellationToken)
    {
        var itemsToUpdate = await dbContext.ShoppingCartItems
            .Where(bi => bi.ProductId == command.ProductId)
            .ToListAsync(cancellationToken);

        if(!itemsToUpdate.Any())
        {
            return new UpdateItemPriceInBasketResult(false);
        }

        foreach (var item in itemsToUpdate)
        {
            item.UpdatePrice(command.Price);
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        return new UpdateItemPriceInBasketResult(true);
    }
}
