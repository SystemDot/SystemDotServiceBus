using SystemDot.Messaging.Messages;

namespace SystemDot.Messaging.Channels.RequestReply.Builders
{
    public interface ISendChannelBuilder
    {
        void Build(EndpointAddress recieverAddress);
    }
}