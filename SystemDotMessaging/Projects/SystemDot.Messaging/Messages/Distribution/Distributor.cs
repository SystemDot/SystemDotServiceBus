using System.Collections.Concurrent;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Messages.Packaging;

namespace SystemDot.Messaging.Messages.Distribution
{
    public class Distributor : IDistributor
    {
        readonly MessagePayloadCopier messagePayloadCopier;
        readonly ConcurrentDictionary<object, IMessageInputter<MessagePayload>> subscribers;
        
        public Distributor(MessagePayloadCopier messagePayloadCopier)
        {
            Contract.Requires(messagePayloadCopier != null);

            this.messagePayloadCopier = messagePayloadCopier;
            this.subscribers = new ConcurrentDictionary<object, IMessageInputter<MessagePayload>>();
        }

        public void InputMessage(MessagePayload toInput)
        {
            this.subscribers.Values.ForEach(s => s.InputMessage(this.messagePayloadCopier.Copy(toInput)));        
        }

        public void Subscribe(object key, IMessageInputter<MessagePayload> toSubscribe)
        {
            this.subscribers.TryAdd(key, toSubscribe);
        }
    }
}