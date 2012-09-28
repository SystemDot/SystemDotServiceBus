using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Caching;
using SystemDot.Messaging.Channels.Distribution;
using SystemDot.Messaging.Channels.Filtering;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Pipelines;
using SystemDot.Messaging.Channels.Repeating;
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
        readonly IPersistence persistence;
        readonly ICurrentDateProvider currentDateProvider;
        readonly ITaskRepeater taskRepeater;
        readonly MessageCacheBuilder messageCacheBuilder;

        public PublisherChannelBuilder(
            IPublisherRegistry publisherRegistry, 
            MessagePayloadCopier messagePayloadCopier, 
            ISerialiser serialiser, 
            IPersistence persistence, 
            ICurrentDateProvider currentDateProvider, 
            ITaskRepeater taskRepeater, 
            MessageCacheBuilder messageCacheBuilder)
        {
            Contract.Requires(publisherRegistry != null);
            Contract.Requires(messagePayloadCopier != null);
            Contract.Requires(serialiser != null);
            Contract.Requires(persistence != null);
            Contract.Requires(currentDateProvider != null);
            Contract.Requires(taskRepeater != null);
            Contract.Requires(messageCacheBuilder != null);

            this.publisherRegistry = publisherRegistry;
            this.messagePayloadCopier = messagePayloadCopier;
            this.serialiser = serialiser;
            this.persistence = persistence;
            this.currentDateProvider = currentDateProvider;
            this.taskRepeater = taskRepeater;
            this.messageCacheBuilder = messageCacheBuilder;
        }

        public void Build(PublisherChannelSchema schema)
        {
            IMessageCache cache = this.messageCacheBuilder.Create(schema);
            
            var publisherEndpoint = new Publisher(this.messagePayloadCopier);

            MessagePipelineBuilder.Build()
                .WithBusPublishTo(new MessageFilter(schema.MessageFilterStrategy))
                .ToConverter(new MessagePayloadPackager(this.serialiser))
                .ToMessageRepeater(cache, this.currentDateProvider, this.taskRepeater)
                .ToProcessor(new MessageCacher(cache))
                .Pump()
                .ToProcessor(publisherEndpoint)
                .ToEndPoint(new MessageDecacher(cache));

            this.publisherRegistry.RegisterPublisher(schema.FromAddress, publisherEndpoint);
        }
    }
}