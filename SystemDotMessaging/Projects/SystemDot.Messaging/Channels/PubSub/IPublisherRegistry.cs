using SystemDot.Messaging.Channels.Messages.Distribution;
using SystemDot.Messaging.MessageTransportation;

namespace SystemDot.Messaging.Channels.PubSub
{
    public interface IPublisherRegistry {
        void RegisterPublisher(Address address, IDistributor distributor);
        IDistributor GetPublisher(Address address);
    }
}