using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Packaging;
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
            this.persistence.AddOrUpdateMessageAndIncrementSequence(toInput);
        }
    }
}