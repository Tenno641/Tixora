using Events.Domain.Events;
using Tixora.Shared.Domain.Common;

namespace Events.Domain.Tickets;

public class TicketType: Entity
{
    public Guid EventId { get; private set; }
    public string Name { get; private set; }
    public string Currency { get; private set; }
    public decimal Price { get; private set; }
    public int Quantity { get; private set; }

    public static TicketType Create(
        Event @event,
        string name,
        string currency,
        decimal price,
        int quantity,
        Guid? id = null)
    {
        TicketType ticketType = new TicketType(
            id: id,
            eventId: @event.Id,
            name: name,
            currency: currency,
            price: price,
            quantity: quantity);

        return ticketType;
    }

    public void UpdatePrice(decimal newPrice)
    {
        if (Price == newPrice)
            return;

        Price = newPrice;

        RaiseDomainEvent(new TicketTypePriceUpdatedEvent(Id, newPrice));
    }

    private TicketType(Guid eventId, string name, string currency, decimal price, int quantity, Guid? id = null) : base(id)
    {
        EventId = eventId;
        Name = name;
        Currency = currency;
        Price = price;
        Quantity = quantity;
    }

    private TicketType() { }
}