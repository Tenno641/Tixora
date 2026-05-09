namespace Events.PublicApi;

public interface IEventsApi
{
    Task<TicketResponse?> GetTicketAsync(Guid id, CancellationToken cancellationToken = default);
}

public class TicketResponse
{
    public Guid Id { get; set; }
    public string Currency { get; set; }
    public decimal Price { get; set; }
};