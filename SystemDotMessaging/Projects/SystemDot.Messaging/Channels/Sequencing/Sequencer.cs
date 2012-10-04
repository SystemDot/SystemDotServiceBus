using System;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Channels.Sequencing
{
    public class Sequencer : IMessageProcessor<MessagePayload, MessagePayload>
    {
        readonly IPersistence persistence;
        readonly EndpointAddress address;

        public Sequencer(IPersistence persistence, EndpointAddress address)
        {
            Contract.Requires(persistence != null);
            Contract.Requires(address != null);
            Contract.Requires(address != EndpointAddress.Empty);
            
            this.persistence = persistence;
            this.address = address;
        }

        public void InputMessage(MessagePayload toInput)
        {
            toInput.SetSequence(this.persistence.GetNextSequence(this.address));
            MessageProcessed(toInput);
        }

        public event Action<MessagePayload> MessageProcessed;
    }
}