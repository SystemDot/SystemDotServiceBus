using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Packaging;
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
            MessagePersistenceId id = toInput.GetPersistenceId();

            if (id.Address == this.persistence.Address) 
                return;
            
            toInput.SetLastPersistenceId(id);
            toInput.SetPersistenceId(this.persistence.Address, this.persistence.UseType);
            
            this.persistence.AddOrUpdateMessage(toInput);
        }
    }
}