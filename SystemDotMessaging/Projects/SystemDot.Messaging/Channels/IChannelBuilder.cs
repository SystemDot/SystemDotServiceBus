using SystemDot.Messaging.Channels.RequestReply;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Packaging;

namespace SystemDot.Messaging.Channels
{
    public interface IChannelBuilder
    {
        void Build(IMessageProcessor<MessagePayload> startPoint, IMessageInputter<MessagePayload> endPoint);
    }
}