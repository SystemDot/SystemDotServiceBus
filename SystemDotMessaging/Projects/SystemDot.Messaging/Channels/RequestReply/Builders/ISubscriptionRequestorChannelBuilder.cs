using SystemDot.Messaging.Messages;

namespace SystemDot.Messaging.Channels.RequestReply.Builders
{
    public interface ISubscriptionRequestorChannelBuilder
    {
        ISubscriptionRequestor Build(EndpointAddress senderAddress, EndpointAddress recieverAddress);
    }
}