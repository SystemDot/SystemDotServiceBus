using SystemDot.Messaging.Messages;

namespace SystemDot.Messaging.Channels.RequestReply.Builders
{
    public interface IReplyRecieveChannelBuilder
    {
        void Build(params IMessageProcessor<object, object>[] hooks);
    }
}