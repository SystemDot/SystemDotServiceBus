using System.Diagnostics.Contracts;
using SystemDot.Logging;
using SystemDot.Messaging.Channels.Addressing;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Packaging.Headers;
using SystemDot.Messaging.Channels.Publishing.Builders;

namespace SystemDot.Messaging.Channels.Publishing
{
    public class SubscriptionRequestHandler : IMessageInputter<MessagePayload>
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
            EndpointAddress fromAddress = message.GetToAddress();

            Logger.Info("Handling request reply subscription request for {0}", schema.SubscriberAddress);

            GetPublisher(fromAddress).Subscribe(schema);
        }

        IPublisher GetPublisher(EndpointAddress address)
        {
            return publisherRegistry.GetPublisher(address);
        }
    }
}