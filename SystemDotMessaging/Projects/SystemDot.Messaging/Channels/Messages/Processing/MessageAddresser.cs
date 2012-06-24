using System;
using SystemDot.Messaging.MessageTransportation;
using SystemDot.Messaging.MessageTransportation.Headers;

namespace SystemDot.Messaging.Channels.Messages.Processing
{
    public class MessageAddresser : IMessageProcessor<MessagePayload, MessagePayload>
    {
        public event Action<MessagePayload> MessageProcessed;

        public void InputMessage(MessagePayload toInput)
        {
            toInput.SetToAddress(Address.Default);
            MessageProcessed(toInput);
        }
    }
}