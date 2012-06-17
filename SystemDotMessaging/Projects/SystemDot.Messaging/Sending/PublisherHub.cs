using System;
using SystemDot.Messaging.MessageTransportation;

namespace SystemDot.Messaging.Sending
{
    public class PublisherHub : IMessageProcessor<MessagePayload, MessagePayload> 
    {
        public void InputMessage(MessagePayload toInput)
        {
            throw new NotImplementedException();
        }

        public event Action<MessagePayload> MessageProcessed;
    }
}