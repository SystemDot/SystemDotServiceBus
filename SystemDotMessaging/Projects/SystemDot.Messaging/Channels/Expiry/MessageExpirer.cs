using System;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Caching;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Channels.Expiry
{
    public class MessageExpirer : IMessageProcessor<MessagePayload, MessagePayload>
    {
        readonly IMessageExpiryStrategy strategy;
        readonly MessageCache messageCache;

        public MessageExpirer(IMessageExpiryStrategy strategy, MessageCache messageCache)
        {
            Contract.Requires(strategy != null);
            Contract.Requires(messageCache != null);

            this.strategy = strategy;
            this.messageCache = messageCache;
        }

        public void InputMessage(MessagePayload toInput)
        {
            if (this.strategy.HasExpired(toInput))
            {
                this.messageCache.Delete(toInput.Id);
                return;
            }

            MessageProcessed(toInput);
        }

        public event Action<MessagePayload> MessageProcessed;
    }
}