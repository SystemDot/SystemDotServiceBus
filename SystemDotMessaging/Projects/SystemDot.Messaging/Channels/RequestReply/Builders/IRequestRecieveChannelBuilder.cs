using SystemDot.Messaging.Messages;

namespace SystemDot.Messaging.Channels.RequestReply.Builders
{
    public interface IRequestRecieveChannelBuilder
    {
        void Build(EndpointAddress replyAddress);
    }
}