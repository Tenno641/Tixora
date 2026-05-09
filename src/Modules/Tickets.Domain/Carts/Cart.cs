namespace Tickets.Domain.Carts;

public class Cart
{
    public Guid CustomerId { get; private set; }
    public readonly List<CartItem> Items = [];

    public static Cart Create(Guid customerId) => new Cart
    {
        CustomerId = customerId
    };
}