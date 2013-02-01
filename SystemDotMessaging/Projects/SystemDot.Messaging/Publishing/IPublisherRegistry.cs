using SystemDot.Messaging.Addressing;

namespace SystemDot.Messaging.Publishing
{
    public interface IPublisherRegistry 
    {
        void RegisterPublisher(EndpointAddress address, IPublisher publisher);
        IPublisher GetPublisher(EndpointAddress address);
    }
}