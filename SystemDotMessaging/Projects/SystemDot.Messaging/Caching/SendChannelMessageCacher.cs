using System.Diagnostics.Contracts;
using SystemDot.Logging;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Caching
{
    class SendChannelMessageCacher : MessageProcessor
    {
        readonly SendMessageCache messageCache;

        public SendChannelMessageCacher(SendMessageCache messageCache)
        {
            Contract.Requires(messageCache != null);
            this.messageCache = messageCache;
        }

        public override void InputMessage(MessagePayload toInput)
        {
            PersistMessage(toInput);
            OnMessageProcessed(toInput);
        }

        void PersistMessage(MessagePayload toInput)
        {
            Logger.Debug("Caching message payload {0}", toInput.Id);

            toInput.SetPersistenceId(messageCache.Address, messageCache.UseType);
            messageCache.AddMessageAndIncrementSequence(toInput);
        }
    }
}