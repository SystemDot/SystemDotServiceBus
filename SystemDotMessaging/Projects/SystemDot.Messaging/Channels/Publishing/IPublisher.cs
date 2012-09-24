using System;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Packaging;

namespace SystemDot.Messaging.Channels.Publishing
{
    [ContractClass(typeof(PublisherContract))]
    public interface IPublisher : IMessageProcessor<MessagePayload, MessagePayload>
    {
        void Subscribe(object key, IMessageInputter<MessagePayload> toSubscribe);
    }

    [ContractClassFor(typeof(IPublisher))]
    public class PublisherContract : IPublisher 
    {
        public void InputMessage(MessagePayload toInput)
        {
            Contract.Requires(toInput != null);
        }

        public void Subscribe(object key, IMessageInputter<MessagePayload> toSubscribe)
        {
            Contract.Requires(key != null);
            Contract.Requires(toSubscribe != null);
        }

        public event Action<MessagePayload> MessageProcessed;

    }
}