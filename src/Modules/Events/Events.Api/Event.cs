namespace Events.Api;

internal sealed class Event
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }
    public required string Location { get; set; }
    public EventState State { get; set; }
}
