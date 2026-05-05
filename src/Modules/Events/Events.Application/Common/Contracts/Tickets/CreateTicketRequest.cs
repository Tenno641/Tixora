namespace Events.Application.Common.Contracts.Tickets;

public record CreateTicketRequest(string Name, string Currency,  decimal Price, int Quantity);