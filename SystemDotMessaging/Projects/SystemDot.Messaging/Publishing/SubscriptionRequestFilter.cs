using System;
using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Publishing
{
    class SubscriptionRequestFilter : IMessageProcessor<MessagePayload, MessagePayload>
    {
        public void InputMessage(MessagePayload toInput)
        {
            if (!toInput.IsSubscriptionRequest()) return;
            MessageProcessed(toInput);
        }

        public event Action<MessagePayload> MessageProcessed;
    }
}