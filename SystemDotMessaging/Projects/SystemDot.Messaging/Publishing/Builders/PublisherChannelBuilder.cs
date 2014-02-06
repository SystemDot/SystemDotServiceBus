using System.Diagnostics.Contracts;
using SystemDot.Core;
using SystemDot.Messaging.Builders;
using SystemDot.Messaging.Caching;
using SystemDot.Messaging.Filtering;
using SystemDot.Messaging.Hooks;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Pipelines;
using SystemDot.Messaging.Repeating;
using SystemDot.Messaging.Sequencing;
using SystemDot.Messaging.Simple;
using SystemDot.Messaging.Storage;
using SystemDot.Parallelism;
using SystemDot.Serialisation;
using SystemDot.Storage.Changes;

namespace SystemDot.Messaging.Publishing.Builders
{
    class PublisherChannelBuilder
    {
        readonly IPublisherRegistry publisherRegistry;
        readonly ISerialiser serialiser;
        readonly ITaskRepeater taskRepeater;
        readonly MessageCacheFactory messageCacheFactory;
        readonly ISubscriberSendChannelBuilder subscriberChannelBuilder;
        readonly ISystemTime systemTime;
        readonly ChangeStore changeStore;
        readonly ICheckpointStrategy checkPointStrategy;

        public PublisherChannelBuilder(
            IPublisherRegistry publisherRegistry, 
            ISerialiser serialiser, 
            ITaskRepeater taskRepeater,
            MessageCacheFactory messageCacheFactory, 
            ISubscriberSendChannelBuilder subscriberChannelBuilder, 
            ISystemTime systemTime, 
            ChangeStore changeStore,
            ICheckpointStrategy checkPointStrategy)
        {
            Contract.Requires(publisherRegistry != null);
            Contract.Requires(serialiser != null);
            Contract.Requires(taskRepeater != null);
            Contract.Requires(messageCacheFactory != null);
            Contract.Requires(subscriberChannelBuilder != null);
            Contract.Requires(systemTime != null);
            Contract.Requires(changeStore != null);
            Contract.Requires(checkPointStrategy != null);
            
            this.publisherRegistry = publisherRegistry;
            this.serialiser = serialiser;
            this.taskRepeater = taskRepeater;
            this.messageCacheFactory = messageCacheFactory;
            this.subscriberChannelBuilder = subscriberChannelBuilder;
            this.systemTime = systemTime;
            this.changeStore = changeStore;
            this.checkPointStrategy = checkPointStrategy;
        }

        public void Build(PublisherChannelSchema schema)
        {
            Publisher publisherEndpoint = CreatePublisher(schema);

            BuildPipeline(schema, CreateCache(schema), publisherEndpoint);
            RegisterPublisher(schema, publisherEndpoint);
            NotifyPublisherChannelBuilt(schema);
        }

        static void NotifyPublisherChannelBuilt(PublisherChannelSchema schema)
        {
            Messenger.Send(new PublisherChannelBuilt {Address = schema.FromAddress});
        }

        void RegisterPublisher(PublisherChannelSchema schema, Publisher publisherEndpoint)
        {
            publisherRegistry.RegisterPublisher(schema.FromAddress, publisherEndpoint);
        }

        void BuildPipeline(PublisherChannelSchema schema, SendMessageCache cache, Publisher publisherEndpoint)
        {
            MessagePipelineBuilder.Build()
                .WithBusPublishTo(new MessageFilter(schema.MessageFilterStrategy))
                .ToProcessor(new MessageHookRunner<object>(schema.Hooks))
                .ToConverter(new MessagePayloadPackager(serialiser))
                .ToProcessor(new MessageHookRunner<MessagePayload>(schema.PostPackagingHooks))
                .ToProcessor(new Sequencer(cache))
                .ToProcessor(new SendChannelMessageCacher(cache))
                .ToSimpleMessageRepeater(cache, systemTime, taskRepeater)
                .ToProcessor(new SendChannelMessageCacheUpdater(cache))
                .ToProcessor(publisherEndpoint)
                .ToEndPoint(new MessageDecacher(cache));
        }

        Publisher CreatePublisher(PublisherChannelSchema schema)
        {
            return new Publisher(schema.FromAddress, subscriberChannelBuilder, changeStore, checkPointStrategy);
        }

        SendMessageCache CreateCache(PublisherChannelSchema schema)
        {
            return messageCacheFactory.BuildSendCache(PersistenceUseType.PublisherSend, schema.FromAddress, schema);
        }
    }
}