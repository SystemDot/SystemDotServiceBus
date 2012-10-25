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
        readonly IPersistence persistence;

        public MessageExpirer(IMessageExpiryStrategy strategy, IPersistence persistence)
        {
            Contract.Requires(strategy != null);
            Contract.Requires(persistence != null);

            this.strategy = strategy;
            this.persistence = persistence;
        }

        public void InputMessage(MessagePayload toInput)
        {
            if (this.strategy.HasExpired(toInput))
            {
                this.persistence.Delete(toInput.Id);
                return;
            }

            MessageProcessed(toInput);
        }

        public event Action<MessagePayload> MessageProcessed;
    }
}