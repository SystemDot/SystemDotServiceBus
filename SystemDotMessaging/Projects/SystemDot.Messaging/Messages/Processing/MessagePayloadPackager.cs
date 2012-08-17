using System;
using System.Diagnostics.Contracts;
using SystemDot.Logging;
using SystemDot.Messaging.Messages.Packaging;
using SystemDot.Messaging.Messages.Packaging.Headers;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Messages.Processing
{
    public class MessagePayloadPackager : IMessageProcessor<object, MessagePayload>
    {
        private readonly ISerialiser serialiser;

        public event Action<MessagePayload> MessageProcessed;

        public MessagePayloadPackager(ISerialiser serialiser)
        {
            Contract.Requires(serialiser != null);
            this.serialiser = serialiser;
        }

        public void InputMessage(object toInput)
        {
            var messagePayload = new MessagePayload();
            messagePayload.SetBody(this.serialiser.Serialise(toInput));

            Logger.Info("Packaging message payload");

            MessageProcessed(messagePayload);
        }
    }
    
}