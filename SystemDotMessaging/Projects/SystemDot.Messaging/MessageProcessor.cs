using System;
using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging
{
    public abstract class MessageProcessor : IMessageProcessor<MessagePayload, MessagePayload>
    {
        protected void OnMessageProcessed(MessagePayload toInput)
        {
            if(MessageProcessed != null)
                MessageProcessed(toInput);
        }

        public event Action<MessagePayload> MessageProcessed;

        public abstract void InputMessage(MessagePayload toInput);
    }
}