using System;
using System.Diagnostics.Contracts;
using SystemDot.Logging;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Packaging
{
    class MessagePayloadUnpackager : IMessageProcessor<MessagePayload, object>
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

            Logger.Debug("Unpackaging message payload");

            object message = Deserialise(toInput.GetBody());
            
            if (message == null) return;

            Logger.Debug("Unpackaged message payload");

            MessageProcessed(message);
        }

        object Deserialise(byte[] toDeserialise)
        {
            try
            {
                return serialiser.Deserialise(toDeserialise);
            }
            catch (CannotDeserialiseException e)
            {
                Logger.Info("Cannot deserialise message: {0}", e.Message);
                return null;
            }
        }
    }
}