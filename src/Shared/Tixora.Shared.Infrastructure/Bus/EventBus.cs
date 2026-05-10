using MassTransit;
using Tixora.Shared.Application.Common.EventBus;

namespace Tixora.Shared.Infrastructure.Bus;

public class EventBus: IEventBus
{
    private readonly IBus _bus;
    
    public EventBus(IBus bus)
    {
        _bus = bus;
    }
    
    public async Task PublishAsync(IIntegrationEvent integrationEvent)
    {
        await _bus.Publish(integrationEvent, integrationEvent.GetType());
    }
}