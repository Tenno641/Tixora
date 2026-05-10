namespace Tixora.Shared.Application.Common.EventBus;

public interface IEventBus
{
    Task PublishAsync(IIntegrationEvent integrationEvent);
}