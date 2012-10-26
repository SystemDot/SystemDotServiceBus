using System;
using System.Diagnostics.Contracts;
using SystemDot.Logging;
using SystemDot.Messaging.Channels.Packaging.Headers;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Channels.Packaging
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

            object message = Deserialise(toInput.GetBody());
            
            if (message == null) return;

            MessageProcessed(message);
        }

        object Deserialise(byte[] toDeserialise)
        {
            try
            {
                return this.serialiser.Deserialise(toDeserialise);
            }
            catch (CannotDeserialiseException e)
            {
                Logger.Error(e.Message);
                return null;
            }
        }
    }
}