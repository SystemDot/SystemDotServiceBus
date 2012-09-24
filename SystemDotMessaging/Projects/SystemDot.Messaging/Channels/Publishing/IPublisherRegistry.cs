using SystemDot.Messaging.Channels.Distribution;

namespace SystemDot.Messaging.Channels.Publishing
{
    public interface IPublisherRegistry {
        void RegisterPublisher(EndpointAddress address, IPublisher publisher);
        IPublisher GetPublisher(EndpointAddress address);
    }
}