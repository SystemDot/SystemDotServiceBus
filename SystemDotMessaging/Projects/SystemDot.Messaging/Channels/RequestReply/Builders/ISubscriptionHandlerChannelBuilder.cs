using SystemDot.Messaging.Messages;

namespace SystemDot.Messaging.Channels.RequestReply.Builders
{
    public interface ISubscriptionHandlerChannelBuilder
    {
        SubscriptionRequestor Build(EndpointAddress address, EndpointAddress recieverAddress);
    }
}