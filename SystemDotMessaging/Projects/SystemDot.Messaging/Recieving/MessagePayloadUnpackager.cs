using System;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.MessageTransportation;
using SystemDot.Messaging.MessageTransportation.Headers;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Recieving
{
    public class MessagePayloadUnpackager : IMessageProcessor<MessagePayload, object>
    {
        private readonly ISerialiser serialiser;

        public event Action<object> MessageProcessed;
        
        public MessagePayloadUnpackager(ISerialiser serialiser)
        {
            Contract.Requires(serialiser != null);
            this.serialiser = serialiser;
        }

        public void InputMessage(MessagePayload toInput)
        {
            MessageProcessed(this.serialiser.Deserialise(toInput.GetBody()));
        }

    }
}