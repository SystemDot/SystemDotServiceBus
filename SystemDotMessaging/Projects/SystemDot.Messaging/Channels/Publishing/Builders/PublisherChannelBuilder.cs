using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Distribution;
using SystemDot.Messaging.Messages.Pipelines;
using SystemDot.Messaging.Messages.Processing;
using SystemDot.Messaging.Messages.Processing.Filtering;

namespace SystemDot.Messaging.Channels.Publishing.Builders
{
    public class PublisherChannelBuilder : IPublisherChannelBuilder
    {
        readonly IPublisherRegistry publisherRegistry;

        public PublisherChannelBuilder(IPublisherRegistry publisherRegistry)
        {
            this.publisherRegistry = publisherRegistry;
        }

        public void Build(EndpointAddress address, IMessageFilterStrategy messageFilterStategy)
        {
            var publisherEndpoint = IocContainer.Resolve<IDistributor>();

            MessagePipelineBuilder.Build()
                .WithBusPublishTo(IocContainer.Resolve<MessageFilter, IMessageFilterStrategy>(messageFilterStategy))
                .Pump()
                .ToConverter(IocContainer.Resolve<MessagePayloadPackager>())
                .ToEndPoint(publisherEndpoint);

            this.publisherRegistry.RegisterPublisher(address, publisherEndpoint);
        }
    }
}