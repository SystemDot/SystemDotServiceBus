using SystemDot.Messaging.Messages;

namespace SystemDot.Messaging.Channels.RequestReply.Builders
{
    public interface IRequestSendChannelBuilder
    {
        void Build(EndpointAddress fromAddress, EndpointAddress recieverAddress);
    }
}