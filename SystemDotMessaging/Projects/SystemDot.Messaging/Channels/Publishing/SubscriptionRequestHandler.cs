using System.Diagnostics.Contracts;
using SystemDot.Logging;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Packaging.Headers;
using SystemDot.Messaging.Channels.Publishing.Builders;

namespace SystemDot.Messaging.Channels.Publishing
{
    public class SubscriptionRequestHandler : IMessageInputter<MessagePayload>
    {
        readonly IPublisherRegistry publisherRegistry;
        readonly SubscriberSendChannelBuilder builder;

        public SubscriptionRequestHandler(IPublisherRegistry publisherRegistry, SubscriberSendChannelBuilder builder)
        {
            Contract.Requires(publisherRegistry != null);
            Contract.Requires(builder != null);
            
            this.publisherRegistry = publisherRegistry;
            this.builder = builder;
        }

        public void InputMessage(MessagePayload message)
        {
            SubscriptionSchema schema = message.GetSubscriptionRequestSchema();
            EndpointAddress fromAddress = message.GetToAddress();

            Logger.Info("Handling request reply subscription request for {0}", schema.SubscriberAddress);

            GetPublisher(fromAddress).Subscribe(
                schema.SubscriberAddress,
                this.builder.BuildChannel(
                    new SubscriberSendChannelSchema
                    {
                        FromAddress = fromAddress,
                        SubscriberAddress = schema.SubscriberAddress,
                        IsPersistent = schema.IsPersistent
                    }));
        }

        IPublisher GetPublisher(EndpointAddress address)
        {
            return publisherRegistry.GetPublisher(address);
        }
    }
}