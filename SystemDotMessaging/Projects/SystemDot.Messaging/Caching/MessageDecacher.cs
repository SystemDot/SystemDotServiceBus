using System.Diagnostics.Contracts;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Caching
{
    class MessageDecacher : MessageProcessor
    {
        readonly IMessageCache messageCache;

        public MessageDecacher(IMessageCache messageCache)
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