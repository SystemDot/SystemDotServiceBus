using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Caching;
using SystemDot.Messaging.Channels.Distribution;
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
        readonly MessagePayloadCopier messagePayloadCopier;
        readonly ISerialiser serialiser;
        readonly ICurrentDateProvider currentDateProvider;
        readonly ITaskRepeater taskRepeater;
        readonly MessageCacheBuilder messageCacheBuilder;
        readonly IPersistenceFactory persistenceFactory;

        public PublisherChannelBuilder(
            IPublisherRegistry publisherRegistry, 
            MessagePayloadCopier messagePayloadCopier, 
            ISerialiser serialiser, 
            ICurrentDateProvider currentDateProvider, 
            ITaskRepeater taskRepeater, 
            MessageCacheBuilder messageCacheBuilder, 
            IPersistenceFactory persistenceFactory)
        {
            Contract.Requires(publisherRegistry != null);
            Contract.Requires(messagePayloadCopier != null);
            Contract.Requires(serialiser != null);
            Contract.Requires(currentDateProvider != null);
            Contract.Requires(taskRepeater != null);
            Contract.Requires(messageCacheBuilder != null);
            Contract.Requires(persistenceFactory != null);

            this.publisherRegistry = publisherRegistry;
            this.messagePayloadCopier = messagePayloadCopier;
            this.serialiser = serialiser;
            this.currentDateProvider = currentDateProvider;
            this.taskRepeater = taskRepeater;
            this.messageCacheBuilder = messageCacheBuilder;
            this.persistenceFactory = persistenceFactory;
        }

        public void Build(PublisherChannelSchema schema)
        {
            IPersistence persistence = this.persistenceFactory.CreatePersistence(
                PersistenceUseType.PublisherSend, 
                schema.FromAddress);

            IMessageCache cache = this.messageCacheBuilder.Create(schema, persistence); 
            
            var publisherEndpoint = new Publisher(this.messagePayloadCopier);

            MessagePipelineBuilder.Build()
                .WithBusPublishTo(new MessageFilter(schema.MessageFilterStrategy))
                .ToConverter(new MessagePayloadPackager(this.serialiser))
                .ToProcessor(new Sequencer(persistence))
                .ToMessageRepeater(cache, this.currentDateProvider, this.taskRepeater)
                .ToProcessor(new MessageCacher(cache))
                .Pump()
                .ToProcessor(publisherEndpoint)
                .ToEndPoint(new MessageDecacher(cache));

            this.publisherRegistry.RegisterPublisher(schema.FromAddress, publisherEndpoint);
        }
    }
}