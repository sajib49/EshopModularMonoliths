using Shared.Data.Seed;

namespace Basket.Data.Seed;

public class BasketDataSeeder(BasketDbContext dbContext) : IDataSeeder
{
    public async Task SeedAllAsync()
    {
        // Seed initial data if necessary
        if (!await dbContext.ShoppingCarts.AnyAsync())
        {
            //await dbContext.ShoppingCarts.AddRangeAsync(InitialData.ShoppingCarts);
            //await dbContext.SaveChangesAsync();
        }
    }
}
