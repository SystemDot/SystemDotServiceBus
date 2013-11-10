using System;
using System.Diagnostics.Contracts;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Packaging
{
    class MessagePayloadCopier : MessageProcessor
    {
        readonly ISerialiser serialiser;

        public MessagePayloadCopier(ISerialiser serialiser)
        {
            Contract.Requires(serialiser != null);
            this.serialiser = serialiser;
        }

        public override void InputMessage(MessagePayload toInput)
        {
            OnMessageProcessed(Copy(toInput));
        }

        MessagePayload Copy(MessagePayload toCopy)
        {
            return serialiser.Copy(toCopy);
        }
    }
}