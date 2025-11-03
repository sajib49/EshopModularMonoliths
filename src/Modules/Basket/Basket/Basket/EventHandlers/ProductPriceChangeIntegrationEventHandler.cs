using Basket.Basket.Features.UpdateItemPriceInBasket;
using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.Messaging.Events;

namespace Basket.Basket.EventHandlers;

public class ProductPriceChangeIntegrationEventHandler
    (ISender sender, ILogger<ProductPriceChangeIntegrationEventHandler> logger)
    : IConsumer<ProductPriceChangedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<ProductPriceChangedIntegrationEvent> context)
    {
        logger.LogInformation("Integration Event handled: {IntegrationEvent}", context.Message.GetType().Name);
       
        var command = new UpdateItemPriceInBasketCommand(
            context.Message.ProductId,
            context.Message.Price);
        var result = await sender.Send(command);

        if (!result.IsSuccess)
        {
            logger.LogWarning("Failed to update item price in basket for ProductId: {ProductId}", context.Message.ProductId);
        }

        logger.LogWarning("Price in basket for ProductId: {ProductId} updated in the basket", context.Message.ProductId);


    }
}
