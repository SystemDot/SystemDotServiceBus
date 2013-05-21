using System;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Expiry
{
    class MessageExpirer : IMessageProcessor<MessagePayload, MessagePayload>
    {
        readonly IMessageExpiryStrategy strategy;
        readonly IMessageCache messageCache;

        public MessageExpirer(IMessageExpiryStrategy strategy, IMessageCache messageCache)
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