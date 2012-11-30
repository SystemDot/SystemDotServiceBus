using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Repeating;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Channels.Caching
{
    public class ReceiveChannelMessageCacher : MessageProcessor
    {
        readonly MessageCache messageCache;

        public ReceiveChannelMessageCacher(MessageCache messageCache)
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