using System;
using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging
{
    public abstract class MessageProcessor : IMessageProcessor<MessagePayload, MessagePayload>
    {
        protected void OnMessageProcessed(MessagePayload toInput)
        {
            if(this.MessageProcessed != null)
                this.MessageProcessed(toInput);
        }

        public event Action<MessagePayload> MessageProcessed;

        public abstract void InputMessage(MessagePayload toInput);
    }
}