using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Channels.Caching
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