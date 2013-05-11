using System.Diagnostics.Contracts;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Caching
{
    class SendChannelMessageCacheUpdater : MessageProcessor
    {
        readonly SendMessageCache messageCache;

        public SendChannelMessageCacheUpdater(SendMessageCache messageCache)
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
            this.messageCache.UpdateMessage(toInput);
        }
    }
}