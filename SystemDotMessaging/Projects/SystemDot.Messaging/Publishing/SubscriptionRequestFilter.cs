using System;
using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Publishing
{
    public class SubscriptionRequestFilter : IMessageProcessor<MessagePayload, MessagePayload>
    {
        public void InputMessage(MessagePayload toInput)
        {
            if (!toInput.IsSubscriptionRequest()) return;
            this.MessageProcessed(toInput);
        }

        public event Action<MessagePayload> MessageProcessed;
    }
}