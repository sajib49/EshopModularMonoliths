using Basket.Basket.Dtos;
using Catalog.Contracts.Products.Features.GetProductById;

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

internal class AddItemIntoBasketHandler(IBasketRepository basketRepository, ISender sender) : ICommandHandler<AddItemIntoBasketCommand, AddItemIntoBasketResult>
{
    public async Task<AddItemIntoBasketResult> Handle(AddItemIntoBasketCommand command, CancellationToken cancellationToken)
    {
       var shoppingCart = await basketRepository.GetBasket(command.UserName, false, cancellationToken);

        var result = await sender.Send(new GetProductByIdQuery(command.ShoppingCartItem.ProductId));

        shoppingCart.AddItem(
            productId: command.ShoppingCartItem.ProductId,
            quantity: command.ShoppingCartItem.Quantity,
            color: command.ShoppingCartItem.Color,
            result.Product.Price,
            result.Product.Name);

        await basketRepository.SaveChangesAsync(command.UserName, cancellationToken);

        return new AddItemIntoBasketResult(shoppingCart.Id);
    }
}
