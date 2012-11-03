using System;
using System.Diagnostics.Contracts;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Channels.Packaging
{
    public class MessagePayloadCopier : IMessageProcessor<MessagePayload, MessagePayload>
    {
        readonly ISerialiser serialiser;

        public MessagePayloadCopier(ISerialiser serialiser)
        {
            Contract.Requires(serialiser != null);
            this.serialiser = serialiser;
        }

        public void InputMessage(MessagePayload toInput)
        {
            MessagePayload copied = Copy(toInput);
            MessageProcessed(copied);
        }

        MessagePayload Copy(MessagePayload toCopy)
        {
            return this.serialiser.Copy(toCopy);
        }

        public event Action<MessagePayload> MessageProcessed;
    }
}