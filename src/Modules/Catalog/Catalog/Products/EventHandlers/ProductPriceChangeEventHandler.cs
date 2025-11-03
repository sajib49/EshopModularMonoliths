using MassTransit;
using Shared.Messaging.Events;

namespace Catalog.Products.EventHandlers;

public class ProductPriceChangeEventHandler(IBus bus,ILogger<ProductPriceChangesEvent> logger)
    : INotificationHandler<ProductPriceChangesEvent>
{
    public async Task Handle(ProductPriceChangesEvent notification, CancellationToken cancellationToken)
    {
       logger.LogInformation("Notification Type {DomainEvent}. Product created: {ProductId}, Price: {Price}",notification.GetType().Name, notification.Product.Id, notification.Product.Price);
        //return Task.CompletedTask;

        var integrationEvent = new ProductPriceChangedIntegrationEvent
        {
            ProductId = notification.Product.Id,
            Name = notification.Product.Name,
            Category = notification.Product.Category,
            Description = notification.Product.Description,
            ImageFile = notification.Product.ImageFile,
            Price = notification.Product.Price //set updated product price
        };

        await bus.Publish(integrationEvent, cancellationToken);

    }
}
