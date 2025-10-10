namespace Basket.Basket.Features.AddItemIntoBasket;

public record AddItemIntoBasketCommand(string UserName, ShoppingCartItemDto ShoppingCartItem)
    : ICommand<AddItemIntoBasketResult>;

public record AddItemIntoBasketResult(Guid Id);

public class AddItemIntoBasketCommandValidator: AbstractValidator<AddItemIntoBasketCommand>
{
    public AddItemIntoBasketCommandValidator()
    {
        RuleFor(x => x.UserName).NotEmpty().WithMessage("UserName is required.");
        RuleFor(x => x.ShoppingCartItem.ProductId).NotEmpty().WithMessage("Product Id is required.");
        RuleFor(x => x.ShoppingCartItem.Quantity).GreaterThan(0).WithMessage("Quantity should be more than zero.");
    }
}

internal class AddItemIntoBasketHandler(IBasketRepository basketRepository) : ICommandHandler<AddItemIntoBasketCommand, AddItemIntoBasketResult>
{
    public async Task<AddItemIntoBasketResult> Handle(AddItemIntoBasketCommand command, CancellationToken cancellationToken)
    {
       var shoppingCart = await basketRepository.GetBasket(command.UserName, false, cancellationToken);

        shoppingCart.AddItem(
            productId: command.ShoppingCartItem.ProductId,
            quantity: command.ShoppingCartItem.Quantity,
            color: command.ShoppingCartItem.Color,
            price: command.ShoppingCartItem.Price,
            productName: command.ShoppingCartItem.ProductName);

        await basketRepository.SaveChangesAsync(cancellationToken);

        return new AddItemIntoBasketResult(shoppingCart.Id);
    }
}
