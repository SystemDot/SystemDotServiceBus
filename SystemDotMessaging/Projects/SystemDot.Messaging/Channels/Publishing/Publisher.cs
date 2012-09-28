using System;
using System.Collections.Concurrent;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Repeating;

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
            this.subscribers.Values.ForEach(s => s.InputMessage(CopyMessage(toInput)));
            MessageProcessed(toInput);
        }

        MessagePayload CopyMessage(MessagePayload toInput)
        {
            MessagePayload copy = this.messagePayloadCopier.Copy(toInput);
            copy.Headers.RemoveAll(h => h is LastSentHeader);
            return copy;
        }

        public void Subscribe(object key, IMessageInputter<MessagePayload> toSubscribe)
        {
            this.subscribers.TryAdd(key, toSubscribe);
        }
    }
}