using System;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.MessageTransportation;
using SystemDot.Messaging.MessageTransportation.Headers;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Channels.Messages.Processing
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
            messagePayload.SetToAddress(Address.Default);
            messagePayload.SetBody(this.serialiser.Serialise(toInput));

            MessageProcessed(messagePayload);
        }
    }
}