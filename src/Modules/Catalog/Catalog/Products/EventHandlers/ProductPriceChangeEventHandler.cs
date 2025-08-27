namespace Catalog.Products.EventHandlers;

public class ProductPriceChangeEventHandler(ILogger<ProductPriceChangesEvent> logger)
    : INotificationHandler<ProductPriceChangesEvent>
{
    public Task Handle(ProductPriceChangesEvent notification, CancellationToken cancellationToken)
    {
       logger.LogInformation("Notification Type {DomainEvent}. Product created: {ProductId}, Price: {Price}",notification.GetType().Name, notification.Product.Id, notification.Product.Price);
        return Task.CompletedTask;
    } 
}
