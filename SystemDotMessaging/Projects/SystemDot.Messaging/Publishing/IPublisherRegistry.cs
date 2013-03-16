using SystemDot.Messaging.Addressing;

namespace SystemDot.Messaging.Publishing
{
    interface IPublisherRegistry 
    {
        void RegisterPublisher(EndpointAddress address, IPublisher publisher);
        IPublisher GetPublisher(EndpointAddress address);
    }
}