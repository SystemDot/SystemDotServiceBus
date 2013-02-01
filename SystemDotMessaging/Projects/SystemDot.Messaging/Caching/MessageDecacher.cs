using System.Diagnostics.Contracts;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Caching
{
    public class MessageDecacher : MessageProcessor
    {
        readonly MessageCache messageCache;

        public MessageDecacher(MessageCache messageCache)
        {
            Contract.Requires(messageCache != null);
            this.messageCache = messageCache;
        }

        public override void InputMessage(MessagePayload toInput)
        {
            this.messageCache.Delete(toInput.Id);
            OnMessageProcessed(toInput);
        }
    }
}