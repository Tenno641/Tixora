namespace Events.Api.Contracts.Tickets;

public record CreateTicketRequest(string Name, string Currency,  decimal Price, int Quantity);