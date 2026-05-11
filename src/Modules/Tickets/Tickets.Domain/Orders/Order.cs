using ErrorOr;
using Evently.Modules.Ticketing.Domain.Orders;
using Tickets.Domain.Customers;
using Tickets.Domain.Events;
using Tixora.Shared.Domain.Common;

namespace Tickets.Domain.Orders;

public sealed class Order : Entity
{
    private readonly List<OrderItem> _orderItems = [];

    public Guid CustomerId { get; private set; }

    public OrderStatus Status { get; private set; }

    public decimal TotalPrice { get; private set; }

    public string Currency { get; private set; }

    public bool TicketsIssued { get; private set; }

    public DateTime CreatedAtUtc { get; private set; }

    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.ToList();

    public static Order Create(Customer customer)
    {
        Order order = new Order
        (
            customer.Id,
            OrderStatus.Pending,
            DateTime.UtcNow
        );

        order.RaiseDomainEvent(new OrderCreatedDomainEvent(order.Id));

        return order;
    }

    public void AddItem(TicketType ticketType, decimal quantity, decimal price, string currency)
    {
        OrderItem orderItem = OrderItem.Create(Id, ticketType.Id, quantity, price, currency);

        _orderItems.Add(orderItem);

        TotalPrice = _orderItems.Sum(o => o.Price);
        
        Currency = currency;
    }

    public ErrorOr<Success> IssueTickets()
    {
        if (TicketsIssued)
            return OrderErrors.TicketsAlreadyIssues;

        TicketsIssued = true;

        RaiseDomainEvent(new OrderTicketsIssuedDomainEvent(Id));

        return Result.Success;
    }
    
    private Order(Guid customerId, OrderStatus status, DateTime createdAtUtc, Guid? id = null) : base(id)
    {
        CustomerId = customerId;
        Status = status;
        CreatedAtUtc = createdAtUtc;
    }
    
    private Order() { }
}