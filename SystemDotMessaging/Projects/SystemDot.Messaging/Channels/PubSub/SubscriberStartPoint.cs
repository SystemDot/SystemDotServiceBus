using System;
using SystemDot.Messaging.Channels.Messages.Distribution;
using SystemDot.Messaging.MessageTransportation;

namespace SystemDot.Messaging.Channels.PubSub
{
    public class SubscriberStartPoint : IDistributionSubscriber, IChannelStartPoint<MessagePayload>
    {
        public void Recieve(MessagePayload message)
        {
            throw new System.NotImplementedException();
        }

        public event Action<MessagePayload> MessageProcessed;
    }
}