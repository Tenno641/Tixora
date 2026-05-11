using ErrorOr;
using Tixora.Shared.Domain.Common;

namespace Tickets.Domain.Events;

public sealed class TicketType : Entity
{
    public Guid EventId { get; private set; }
    public string Name { get; private set; }
    public decimal Price { get; private set; }
    public string Currency { get; private set; }
    public decimal Quantity { get; private set; }
    public decimal AvailableQuantity { get; private set; }

    public static TicketType Create(
        Guid eventId,
        string name,
        decimal price,
        string currency,
        decimal quantity,
        Guid? id = null)
    {
        TicketType ticketType = new TicketType(
            eventId,
            name,
            price,
            currency,
            quantity,
            quantity,
            id);

        return ticketType;
    }

    public void UpdatePrice(decimal price)
    {
        Price = price;
    }

    public ErrorOr<Success> UpdateQuantity(decimal quantity)
    {
        if (AvailableQuantity < quantity)
            return TicketTypeErrors.NotEnoughQuantity(AvailableQuantity);

        AvailableQuantity -= quantity;

        if (AvailableQuantity == 0)
        {
            RaiseDomainEvent(new TicketTypeSoldOutDomainEvent(Id));
        }

        return Result.Success;
    }

    private TicketType(Guid eventId, string name, decimal price, string currency, decimal quantity, decimal availableQuantity, Guid? id = null) : base(id)
    {
        EventId = eventId;
        Name = name;
        Price = price;
        Currency = currency;
        Quantity = quantity;
        AvailableQuantity = availableQuantity;
    }
    
    private TicketType() { }
}
