using System.Diagnostics.Contracts;
using SystemDot.Logging;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;

namespace SystemDot.Messaging.Publishing
{
    class SubscriptionRequestHandler : IMessageInputter<MessagePayload>
    {
        readonly IPublisherRegistry publisherRegistry;
        
        public SubscriptionRequestHandler(IPublisherRegistry publisherRegistry)
        {
            Contract.Requires(publisherRegistry != null);
            
            this.publisherRegistry = publisherRegistry;
        }

        public void InputMessage(MessagePayload message)
        {
            SubscriptionSchema schema = message.GetSubscriptionRequestSchema();

            Logger.Info("Handling request reply subscription request for {0}", schema.SubscriberAddress);

            GetPublisher(message.GetToAddress()).Subscribe(schema);
        }

        IPublisher GetPublisher(EndpointAddress address)
        {
            return publisherRegistry.GetPublisher(address);
        }
    }
}