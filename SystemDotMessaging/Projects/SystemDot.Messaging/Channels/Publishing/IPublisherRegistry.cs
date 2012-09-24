using SystemDot.Messaging.Channels.Distribution;

namespace SystemDot.Messaging.Channels.Publishing
{
    public interface IPublisherRegistry {
        void RegisterPublisher(EndpointAddress address, IDistributor distributor);
        IDistributor GetPublisher(EndpointAddress address);
    }
}