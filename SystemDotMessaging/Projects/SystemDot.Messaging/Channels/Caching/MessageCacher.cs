using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Channels.Caching
{
    public class MessageCacher : MessageProcessor
    {
        readonly MessageCache messageCache;

        public MessageCacher(MessageCache messageCache)
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
            toInput.SetPersistenceId(this.messageCache.Address, this.messageCache.UseType);
            this.messageCache.AddMessage(toInput);
        }
    }
}