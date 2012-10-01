using System;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Caching;
using SystemDot.Messaging.Channels.Packaging;

namespace SystemDot.Messaging.Channels.Expiry
{
    public class MessageExpirer : IMessageProcessor<MessagePayload, MessagePayload>
    {
        readonly IMessageExpiryStrategy strategy;
        readonly IMessageCache cache;

        public MessageExpirer(IMessageExpiryStrategy strategy, IMessageCache cache)
        {
            Contract.Requires(strategy != null);
            Contract.Requires(cache != null);

            this.strategy = strategy;
            this.cache = cache;
        }

        public void InputMessage(MessagePayload toInput)
        {
            if (this.strategy.HasExpired(toInput))
            {
                this.cache.Remove(toInput.Id);
                return;
            }

            MessageProcessed(toInput);
        }

        public event Action<MessagePayload> MessageProcessed;
    }
}