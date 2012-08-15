using System;
using System.Diagnostics.Contracts;

namespace SystemDot.Messaging.Messages.Processing
{
    public class MessageReplyFilter : IMessageProcessor<object, object>
    {
        readonly Guid channelIdentifier;
        readonly ThreadLocalChannelReserve channelReserve;

        public MessageReplyFilter(Guid channelIdentifier, ThreadLocalChannelReserve channelReserve)
        {
            Contract.Requires(channelIdentifier != Guid.Empty);
            Contract.Requires(channelReserve != null);
            
            this.channelIdentifier = channelIdentifier;
            this.channelReserve = channelReserve;
        }

        public void InputMessage(object toInput)
        {
            Contract.Requires(toInput != null);

            if (this.channelReserve.ReservedChannel != channelIdentifier) return;
            MessageProcessed(toInput);
        }

        public event Action<object> MessageProcessed;
    }
}