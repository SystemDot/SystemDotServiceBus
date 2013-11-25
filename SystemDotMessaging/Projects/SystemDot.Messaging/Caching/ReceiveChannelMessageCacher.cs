using System.Diagnostics.Contracts;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Caching
{
    class ReceiveChannelMessageCacher : MessageProcessor
    {
        readonly ReceiveMessageCache messageCache;

        public ReceiveChannelMessageCacher(ReceiveMessageCache messageCache)
        {
            Contract.Requires(messageCache != null);
            this.messageCache = messageCache;
        }

        public override void InputMessage(MessagePayload toInput)
        {
            StampLocalPersistenceIdOnMessage(toInput);
            CacheMessage(toInput);
            OnMessageProcessed(toInput);
        }

        void StampLocalPersistenceIdOnMessage(MessagePayload toInput)
        {
            toInput.SetPersistenceId(messageCache.Address, messageCache.UseType);
        }

        void CacheMessage(MessagePayload toInput)
        {
            if(messageCache.ContainsMessage(toInput))
                messageCache.UpdateMessage(toInput);
            else 
                messageCache.AddMessage(toInput);
        }
    }
}