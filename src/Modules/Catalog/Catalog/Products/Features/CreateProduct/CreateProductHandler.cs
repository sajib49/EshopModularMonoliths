namespace Catalog.Products.Features.CreateProduct;

public record CreateProductCommand
    (ProductDto Product)
    : ICommand<CreateProductResult>;

public record CreateProductResult(Guid Id);

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Product.Name).NotEmpty().WithMessage("Please enter product name");
        RuleFor(x => x.Product.Category).NotEmpty().WithMessage("Please enter product category");
        RuleFor(x => x.Product.ImageFile).NotEmpty().WithMessage("Please enter product image file");
        RuleFor(x => x.Product.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
    }
}

internal class CreateProductHandler(
    CatalogDbContext dbContext,
    ILogger<CreateProductHandler> logger)
    : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating a new product with name: {ProductName}", command.Product.Name);

        var product = CreateProductNewProduct(command.Product);
        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new CreateProductResult(product.Id);
    }

    private Product CreateProductNewProduct(ProductDto product)
    {
        return Product.Create(
            Guid.NewGuid(),
            product.Name,
            product.Category,
            product.Description,
            product.ImageFile,
            product.Price
        );
    }
}
