using System.Diagnostics.Contracts;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Repeating;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Caching
{
    public class ReceiveChannelMessageCacher : MessageProcessor
    {
        readonly ReceiveMessageCache messageCache;

        public ReceiveChannelMessageCacher(ReceiveMessageCache messageCache)
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
            if (toInput.GetAmountSent() == 1)
                this.messageCache.AddMessage(toInput);
            else
                this.messageCache.UpdateMessage(toInput);
        }
    }
}