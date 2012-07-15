using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.RequestReply;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Packaging;

namespace SystemDot.Messaging.Specifications.configuration.request_reply
{
    public class TestChannelBuilder : IChannelBuilder
    {
        public IMessageProcessor<MessagePayload> StartPoint { get; private set; }

        public IMessageInputter<MessagePayload> EndPoint { get; private set; }

        public ChannelBuildSchema Schema { get; private set; }

        public void Build(IMessageProcessor<MessagePayload> startPoint, IMessageInputter<MessagePayload> endPoint)
        {
            StartPoint = startPoint;
            EndPoint = endPoint;
        }
    }
}