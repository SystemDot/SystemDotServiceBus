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
        readonly ISystemTime systemTime;

        public event Action<MessagePayload> MessageProcessed;

        public MessagePayloadPackager(ISerialiser serialiser, ISystemTime systemTime)
        {
            Contract.Requires(serialiser != null);
            Contract.Requires(systemTime != null);

            this.serialiser = serialiser;
            this.systemTime = systemTime;
        }

        public void InputMessage(object toInput)
        {
            var messagePayload = new MessagePayload(systemTime.GetCurrentDate());
            messagePayload.SetBody(serialiser.Serialise(toInput));

            Logger.Debug("Packaging message payload");

            this.MessageProcessed(messagePayload);
        }
    }
    
}