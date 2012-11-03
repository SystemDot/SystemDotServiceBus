using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Repeating;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Channels.Caching
{
    public class ReceiveChannelMessageCacher : MessageProcessor
    {
        readonly IPersistence persistence;

        public ReceiveChannelMessageCacher(IPersistence persistence)
        {
            Contract.Requires(persistence != null);
            this.persistence = persistence;
        }

        public override void InputMessage(MessagePayload toInput)
        {
            PersistMessage(toInput);
            OnMessageProcessed(toInput);
        }

        void PersistMessage(MessagePayload toInput)
        {
            toInput.SetPersistenceId(this.persistence.Address, this.persistence.UseType);
            if (toInput.GetAmountSent() == 1)
                this.persistence.AddMessage(toInput);
            else
                this.persistence.UpdateMessage(toInput);
        }
    }
}