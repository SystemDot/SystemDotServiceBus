using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Channels.Sequencing
{
    public class StartSequenceApplier : MessageProcessor
    {
        readonly IPersistence persistence;

        public StartSequenceApplier(IPersistence persistence)
        {
            this.persistence = persistence;
        }

        public override void InputMessage(MessagePayload toInput)
        {
            if (this.persistence.GetSequence() < toInput.GetFirstSequence())
                this.persistence.SetSequence(toInput.GetFirstSequence());

            OnMessageProcessed(toInput);
        }
    }
}