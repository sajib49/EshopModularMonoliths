using Shared.Exceptions;

namespace Basket.Data.Exceptions;

public class BasketNotFoundException(string userName) : NotFoundException("Shopping Cart", userName)
{
}
