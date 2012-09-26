using System;
using System.Collections.Generic;
using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Publishing;

namespace SystemDot.Messaging.Specifications
{
    public class TestPublisher : IPublisher 
    {
        public List<IMessageInputter<MessagePayload>> Subscribers { get; private set; }

        public TestPublisher()
        {
            Subscribers = new List<IMessageInputter<MessagePayload>>();
        }

        public void Subscribe(object key, IMessageInputter<MessagePayload> toSubscribe)
        {
            this.Subscribers.Add(toSubscribe);
        }

        public void InputMessage(MessagePayload toInput)
        {
            MessageProcessed(toInput);
        }

        public event Action<MessagePayload> MessageProcessed;
    }
}