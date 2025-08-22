using Catalog.Products.Features.GetProductByCategory;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Catalog.Products.Features.GetProductById;

public record GetProductByIdQuery(Guid id)
    : IQuery<GetProductByIdResult>;

public record GetProductByIdResult(ProductDto Product);

internal class GetProductByIdHandler(CatalogDbContext dbContext)
    : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
{
    public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        var product = await dbContext.Products
             .AsNoTracking()
             .SingleOrDefaultAsync(p => p.Id == query.id, cancellationToken);

        var productDto = product.Adapt<ProductDto>();

        return new GetProductByIdResult(productDto);
    }
}
