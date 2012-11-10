using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Builders;
using SystemDot.Messaging.Channels.Caching;
using SystemDot.Messaging.Channels.Filtering;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Pipelines;
using SystemDot.Messaging.Channels.Repeating;
using SystemDot.Messaging.Channels.Sequencing;
using SystemDot.Messaging.Storage;
using SystemDot.Parallelism;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Channels.Publishing.Builders
{
    public class PublisherChannelBuilder
    {
        readonly IPublisherRegistry publisherRegistry;
        readonly ISerialiser serialiser;
        readonly ITaskRepeater taskRepeater;
        readonly PersistenceFactorySelector persistenceFactorySelector;
        readonly ISubscriberSendChannelBuilder subscriberChannelBuilder;
        readonly ICurrentDateProvider currentDateProvider;

        public PublisherChannelBuilder(
            IPublisherRegistry publisherRegistry, 
            ISerialiser serialiser, 
            ITaskRepeater taskRepeater,
            PersistenceFactorySelector persistenceFactorySelector, 
            ISubscriberSendChannelBuilder subscriberChannelBuilder, 
            ICurrentDateProvider currentDateProvider)
        {
            Contract.Requires(publisherRegistry != null);
            Contract.Requires(serialiser != null);
            Contract.Requires(taskRepeater != null);
            Contract.Requires(persistenceFactorySelector != null);
            Contract.Requires(subscriberChannelBuilder != null);
            Contract.Requires(currentDateProvider != null);
            
            this.publisherRegistry = publisherRegistry;
            this.serialiser = serialiser;
            this.taskRepeater = taskRepeater;
            this.persistenceFactorySelector = persistenceFactorySelector;
            this.subscriberChannelBuilder = subscriberChannelBuilder;
            this.currentDateProvider = currentDateProvider;
        }

        public void Build(PublisherChannelSchema schema)
        {
            IPersistence persistence = this.persistenceFactorySelector
                .Select(schema)
                .CreatePersistence(PersistenceUseType.PublisherSend, schema.FromAddress);

            var publisherEndpoint = new Publisher(schema.FromAddress, this.subscriberChannelBuilder);

            MessagePipelineBuilder.Build()
                .WithBusPublishTo(new MessageFilter(schema.MessageFilterStrategy))
                .ToConverter(new MessagePayloadPackager(this.serialiser))
                .ToProcessor(new Sequencer(persistence))
                .ToEscalatingTimeMessageRepeater(persistence, this.currentDateProvider, this.taskRepeater)
                .ToProcessor(new SendChannelMessageCacher(persistence))
                .Pump()
                .ToProcessor(publisherEndpoint)
                .ToEndPoint(new MessageDecacher(persistence));

            this.publisherRegistry.RegisterPublisher(schema.FromAddress, publisherEndpoint);
        }
    }
}