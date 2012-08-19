using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Distribution;
using SystemDot.Messaging.Messages.Packaging;
using SystemDot.Messaging.Messages.Pipelines;
using SystemDot.Messaging.Messages.Processing;
using SystemDot.Messaging.Messages.Processing.Filtering;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Channels.Publishing.Builders
{
    public class PublisherChannelBuilder : IPublisherChannelBuilder
    {
        readonly IPublisherRegistry publisherRegistry;
        readonly MessagePayloadCopier messagePayloadCopier;
        readonly ISerialiser serialiser;

        public PublisherChannelBuilder(
            IPublisherRegistry publisherRegistry, 
            MessagePayloadCopier messagePayloadCopier, 
            ISerialiser serialiser)
        {
            this.publisherRegistry = publisherRegistry;
            this.messagePayloadCopier = messagePayloadCopier;
            this.serialiser = serialiser;
        }

        public void Build(EndpointAddress address, IMessageFilterStrategy messageFilterStrategy)
        {
            var publisherEndpoint = new Distributor(this.messagePayloadCopier);

            MessagePipelineBuilder.Build()
                .WithBusPublishTo(new MessageFilter(messageFilterStrategy))
                .Pump()
                .ToConverter(new MessagePayloadPackager(this.serialiser))
                .ToEndPoint(publisherEndpoint);

            this.publisherRegistry.RegisterPublisher(address, publisherEndpoint);
        }
    }
}