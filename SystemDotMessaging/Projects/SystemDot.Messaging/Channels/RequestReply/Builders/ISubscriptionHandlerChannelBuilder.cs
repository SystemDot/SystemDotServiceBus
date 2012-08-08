using SystemDot.Messaging.Messages;

namespace SystemDot.Messaging.Channels.RequestReply.Builders
{
    public interface ISubscriptionHandlerChannelBuilder
    {
        ISubscriptionRequestor Build(EndpointAddress address, EndpointAddress recieverAddress);
    }
}