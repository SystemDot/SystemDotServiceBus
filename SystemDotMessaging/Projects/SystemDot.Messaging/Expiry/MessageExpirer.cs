using System;
using System.Diagnostics.Contracts;
using SystemDot.Logging;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Expiry
{
    class MessageExpirer : IMessageProcessor<MessagePayload, MessagePayload>
    {
        readonly IMessageExpiryStrategy strategy;
        readonly Action expiryAction;
        readonly IMessageCache messageCache;

        public MessageExpirer(IMessageExpiryStrategy strategy, Action expiryAction, IMessageCache messageCache)
        {
            Contract.Requires(strategy != null);
            Contract.Requires(expiryAction != null);
            Contract.Requires(messageCache != null);

            this.strategy = strategy;
            this.expiryAction = expiryAction;
            this.messageCache = messageCache;
        }

        public void InputMessage(MessagePayload toInput)
        {
            if (strategy.HasExpired(toInput))
            {
                Logger.Debug("Expiring message payload {0}", toInput.Id);

                messageCache.Delete(toInput.Id);
                expiryAction();
                return;
            }

            MessageProcessed(toInput);
        }

        public event Action<MessagePayload> MessageProcessed;
    }
}