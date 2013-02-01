using System;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Publishing
{
    [ContractClass(typeof(PublisherContract))]
    public interface IPublisher : IMessageProcessor<MessagePayload, MessagePayload>
    {
        void Subscribe(SubscriptionSchema scheam);
    }

    [ContractClassFor(typeof(IPublisher))]
    public class PublisherContract : IPublisher 
    {
        public void InputMessage(MessagePayload toInput)
        {
            Contract.Requires(toInput != null);
        }

        public void Subscribe(SubscriptionSchema schema)
        {        
            Contract.Requires(schema != null);
        }

        public event Action<MessagePayload> MessageProcessed;

    }
}