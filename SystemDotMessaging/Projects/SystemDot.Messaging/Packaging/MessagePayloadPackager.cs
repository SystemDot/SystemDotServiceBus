using System;
using System.Diagnostics.Contracts;
using SystemDot.Logging;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Packaging
{
    class MessagePayloadPackager : IMessageProcessor<object, MessagePayload>
    {
        readonly ISerialiser serialiser;

        public event Action<MessagePayload> MessageProcessed;

        public MessagePayloadPackager(ISerialiser serialiser)
        {
            Contract.Requires(serialiser != null);

            this.serialiser = serialiser;
        }

        public void InputMessage(object toInput)
        {
            var messagePayload = new MessagePayload();
            messagePayload.SetBody(serialiser.Serialise(toInput));

            Logger.Debug("Packaging message payload");

            this.MessageProcessed(messagePayload);
        }
    }
    
}