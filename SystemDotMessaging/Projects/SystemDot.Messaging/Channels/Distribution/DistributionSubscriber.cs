using System;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.MessageTransportation;

namespace SystemDot.Messaging.Channels.Distribution
{
    public class DistributionSubscriber : IChannelStartPoint<MessagePayload>
    {
        public void Update(MessagePayload message)
        {
            Contract.Requires(message != null);
            MessageProcessed(message);
        }

        public event Action<MessagePayload> MessageProcessed;
    }
}