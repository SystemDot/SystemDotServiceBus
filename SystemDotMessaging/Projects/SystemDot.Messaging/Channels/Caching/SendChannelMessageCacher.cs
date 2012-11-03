using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Repeating;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Channels.Caching
{
    public class SendChannelMessageCacher : MessageProcessor
    {
        readonly IPersistence persistence;

        public SendChannelMessageCacher(IPersistence persistence)
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
            toInput.SetSourcePersistenceId(toInput.GetPersistenceId());

            if(toInput.GetAmountSent() == 1)
                this.persistence.AddMessageAndIncrementSequence(toInput);
            else    
                this.persistence.UpdateMessage(toInput);
        }
    }
}