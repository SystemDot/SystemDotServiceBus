using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Channels.Sequencing
{
    public class FirstMessageSequenceRecorder : MessageProcessor
    {
        readonly IPersistence persistence;

        public FirstMessageSequenceRecorder(IPersistence persistence)
        {
            Contract.Requires(persistence != null);
            this.persistence = persistence;
        }

        public override void InputMessage(MessagePayload toInput)
        {
            if (!this.persistence.HasChanged()) 
                this.persistence.SetSequence(toInput.GetSequence());
            
            toInput.SetFirstSequence(this.persistence.GetSequence());

            OnMessageProcessed(toInput);
        }
    }
}