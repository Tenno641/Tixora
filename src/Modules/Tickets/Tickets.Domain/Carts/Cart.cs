namespace Tickets.Domain.Carts;

public class Cart
{
    public Guid CustomerId { get; set; }
    public List<CartItem> Items { get; set; } = [];

    public static Cart Create(Guid customerId) => new Cart
    {
        CustomerId = customerId
    };
}