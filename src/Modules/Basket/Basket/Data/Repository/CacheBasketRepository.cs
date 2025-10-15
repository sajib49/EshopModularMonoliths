using Basket.Data.JsonConverter;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Basket.Data.Repository;

public class CacheBasketRepository(IBasketRepository repository,
    IDistributedCache cache) 
    : IBasketRepository
{
    private readonly JsonSerializerOptions _options = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        Converters =
                {
                    new ShoppingCartConverter(),
                    new ShoppingCartItemConverter()
                }
    };

    public async Task<ShoppingCart> GetBasket(string UserName, bool asNoTracking = true, CancellationToken cancellationToken = default)
    {
        if (!asNoTracking)
        {
            return await repository.GetBasket(UserName, false, cancellationToken);
        }

        var cachedBasket = await cache.GetStringAsync(UserName, cancellationToken);
        if (!string.IsNullOrEmpty(cachedBasket)) 
        {
            return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket, _options)!;
        }

        var basket = await repository.GetBasket(UserName, asNoTracking, cancellationToken);

        await cache.SetStringAsync(UserName, JsonSerializer.Serialize(basket, _options), cancellationToken);

        return basket;
    }

    public async Task<ShoppingCart> CreateBasket(ShoppingCart basket, CancellationToken cancellationToken = default)
    {
        await repository.CreateBasket(basket, cancellationToken);
        await cache.SetStringAsync(basket.UserName, JsonSerializer.Serialize(basket, _options), cancellationToken);   
        return basket;
    }

    public async Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default)
    {
         await repository.DeleteBasket(userName, cancellationToken);
        await cache.RemoveAsync(userName, cancellationToken);
        return true;
    }       

    public async Task<int> SaveChangesAsync(string? userName = null, CancellationToken cancellationToken = default)
    {
        var result =  await repository.SaveChangesAsync(userName, cancellationToken);

        //TODO: Clear cache

        if(userName is not null)
        {
            await cache.RemoveAsync(userName, cancellationToken);
        }

        return result;
    }
}
