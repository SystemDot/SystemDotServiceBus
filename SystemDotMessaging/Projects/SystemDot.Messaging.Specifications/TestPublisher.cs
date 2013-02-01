using System;
using System.Collections.Generic;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Publishing;

namespace SystemDot.Messaging.Specifications
{
    public class TestPublisher : IPublisher 
    {
        public List<SubscriptionSchema> Subscribers { get; private set; }

        public TestPublisher()
        {
            Subscribers = new List<SubscriptionSchema>();
        }

        public void Subscribe(SubscriptionSchema schema)
        {
            this.Subscribers.Add(schema);
        }

        public void InputMessage(MessagePayload toInput)
        {
            MessageProcessed(toInput);
        }

        public event Action<MessagePayload> MessageProcessed;
    }
}