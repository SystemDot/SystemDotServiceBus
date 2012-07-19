using System;
using System.Diagnostics.Contracts;
using SystemDot.Logging;
using SystemDot.Messaging.Messages.Packaging;
using SystemDot.Messaging.Messages.Packaging.Headers;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Messages.Processing
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
            if (!toInput.HasBody()) return;

            Logger.Info("Unpackaging message payload");

            MessageProcessed(this.serialiser.Deserialise(toInput.GetBody()));
        }

    }
}