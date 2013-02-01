using System;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Expiry
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

            this.MessageProcessed(toInput);
        }

        public event Action<MessagePayload> MessageProcessed;
    }
}