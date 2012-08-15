using System;
using System.Diagnostics.Contracts;

namespace SystemDot.Messaging.Messages.Processing
{
    public class ReplyChannelReserver : IMessageProcessor<object, object>
    {
        readonly Guid channelIdentifier;
        readonly ThreadLocalChannelReserve channelReserve;

        public ReplyChannelReserver(Guid channelIdentifier, ThreadLocalChannelReserve channelReserve)
        {
            Contract.Requires(channelIdentifier != Guid.Empty);
            Contract.Requires(channelReserve != null);

            this.channelIdentifier = channelIdentifier;
            this.channelReserve = channelReserve;
        }

        public void InputMessage(object toInput)
        {
            Contract.Requires(toInput != null);
            
            this.channelReserve.ReservedChannel = this.channelIdentifier;
            MessageProcessed(toInput);
        }

        public event Action<object> MessageProcessed;
    }
}