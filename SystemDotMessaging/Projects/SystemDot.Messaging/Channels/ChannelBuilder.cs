using System;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Packaging;

namespace SystemDot.Messaging.Channels
{
    public class ChannelBuilder : IChannelBuilder
    {
        public void Build(IMessageProcessor<MessagePayload> startPoint, IMessageInputter<MessagePayload> endPoint)
        {
            throw new NotImplementedException();
        }
    }
}