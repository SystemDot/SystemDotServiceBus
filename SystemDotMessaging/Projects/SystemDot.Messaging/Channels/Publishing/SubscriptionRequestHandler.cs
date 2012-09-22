using System;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Packaging;

namespace SystemDot.Messaging.Channels.Publishing
{
    public class SubscriptionRequestHandler : IMessageProcessor<MessagePayload, MessagePayload>
    {
        public void InputMessage(MessagePayload toInput)
        {
            if (!toInput.IsSubscriptionRequest()) return;
            MessageProcessed(toInput);
        }

        public event Action<MessagePayload> MessageProcessed;
    }
}