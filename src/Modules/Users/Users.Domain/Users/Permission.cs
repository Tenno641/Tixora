namespace Users.Domain.Users;

public sealed class Permission
{
    public readonly static Permission GetUser = new("users:read");
    public readonly static Permission ModifyUser = new("users:update");
    public readonly static Permission GetEvents = new("events:read");
    public readonly static Permission SearchEvents = new("events:search");
    public readonly static Permission ModifyEvents = new("events:update");
    public readonly static Permission GetTicketTypes = new("ticket-types:read");
    public readonly static Permission ModifyTicketTypes = new("ticket-types:update");
    public readonly static Permission GetCategories = new("categories:read");
    public readonly static Permission ModifyCategories = new("categories:update");
    public readonly static Permission GetCart = new("carts:read");
    public readonly static Permission AddToCart = new("carts:add");
    public readonly static Permission RemoveFromCart = new("carts:remove");
    public readonly static Permission GetOrders = new("orders:read");
    public readonly static Permission CreateOrder = new("orders:create");
    public readonly static Permission GetTickets = new("tickets:read");
    public readonly static Permission CheckInTicket = new("tickets:check-in");
    public readonly static Permission GetEventStatistics = new("event-statistics:read");

    public Permission(string code)
    {
        Code = code;
    }

    public string Code { get; }
}
