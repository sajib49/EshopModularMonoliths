

namespace Catalog.Products.EventHandlers;


public class ProductCreatedEventHandler(ILogger<ProductCreatedEventHandler> logger)
    : INotificationHandler<ProductCreatedEvent>
{
    public Task Handle(ProductCreatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Notification Type {DomainEvent}. Product created: {ProductId}, Price: {Price}", notification.GetType().Name, notification.Product.Id, notification.Product.Price);
        return Task.CompletedTask;
    }
}