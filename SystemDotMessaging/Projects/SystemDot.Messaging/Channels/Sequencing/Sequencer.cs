using System;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Channels.Sequencing
{
    public class Sequencer : IMessageProcessor<MessagePayload, MessagePayload>
    {
        readonly IPersistence persistence;

        public Sequencer(IPersistence persistence)
        {
            Contract.Requires(persistence != null);
            
            this.persistence = persistence;
        }

        public void InputMessage(MessagePayload toInput)
        {
            toInput.SetSequence(this.persistence.GetSequence());
            MessageProcessed(toInput);
        }

        public event Action<MessagePayload> MessageProcessed;
    }
}