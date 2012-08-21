using SystemDot.Messaging.Messages;

namespace SystemDot.Messaging.Channels.Publishing.Builders
{
    public interface ISubscriberChannelBuilder
    {
        void Build(EndpointAddress subscriberAddress);
    }
}