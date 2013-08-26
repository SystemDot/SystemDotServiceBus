using System;
using System.Diagnostics.Contracts;
using System.Linq;
using SystemDot.Logging;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Expiry
{
    class MessageExpirer : IMessageProcessor<MessagePayload, MessagePayload>
    {
        readonly IMessageExpiryStrategy[] strategies;
        readonly Action expiryAction;
        readonly IMessageCache messageCache;

        public MessageExpirer(Action expiryAction, IMessageCache messageCache, params IMessageExpiryStrategy[] strategies)
        {
            Contract.Requires(strategies != null);
            Contract.Requires(expiryAction != null);
            Contract.Requires(messageCache != null);

            this.strategies = strategies;
            this.expiryAction = expiryAction;
            this.messageCache = messageCache;
        }

        public void InputMessage(MessagePayload toInput)
        {
            if (strategies.Any(strategy => strategy.HasExpired(toInput)))
                ExpireMessage(toInput);
            else 
                MessageProcessed(toInput);
        }

        void ExpireMessage(MessagePayload toInput)
        {
            Logger.Debug("Expiring message payload {0}", toInput.Id);

            messageCache.Delete(toInput.Id);
            expiryAction();
        }

        public event Action<MessagePayload> MessageProcessed;
    }
}