namespace Catalog.Products.Events;

public record ProductPriceChangesEvent(Product Product)
    : IDomainEvent;
