using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Distribution;
using SystemDot.Messaging.Messages.Packaging;

namespace SystemDot.Messaging.Channels.Publishing
{
    public interface IPublisherRegistry {
        void RegisterPublisher(EndpointAddress address, IDistributor distributor);
        IDistributor GetPublisher(EndpointAddress address);
    }
}