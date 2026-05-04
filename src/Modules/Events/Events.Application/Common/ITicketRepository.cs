namespace Events.Application.Common;

public interface ITicketRepository
{
    Task<bool> ExistsAsync(Guid id);
}