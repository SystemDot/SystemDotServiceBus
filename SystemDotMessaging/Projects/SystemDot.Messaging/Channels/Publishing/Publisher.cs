using System;
using System.Collections.Concurrent;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Packaging;

namespace SystemDot.Messaging.Channels.Publishing
{
    public class Publisher : IPublisher
    {
        readonly MessagePayloadCopier messagePayloadCopier;
        readonly ConcurrentDictionary<object, IMessageInputter<MessagePayload>> subscribers;

        public event Action<MessagePayload> MessageProcessed;

        public Publisher(MessagePayloadCopier messagePayloadCopier)
        {
            Contract.Requires(messagePayloadCopier != null);

            this.messagePayloadCopier = messagePayloadCopier;
            this.subscribers = new ConcurrentDictionary<object, IMessageInputter<MessagePayload>>();
        }

        public void InputMessage(MessagePayload toInput)
        {
            this.subscribers.Values.ForEach(s => s.InputMessage(this.messagePayloadCopier.Copy(toInput)));
            MessageProcessed(toInput);
        }

        public void Subscribe(object key, IMessageInputter<MessagePayload> toSubscribe)
        {
            this.subscribers.TryAdd(key, toSubscribe);
        }
    }
}