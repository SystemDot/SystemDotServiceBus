using System.Diagnostics.Contracts;
using SystemDot.Messaging.Acknowledgement;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Authentication;
using SystemDot.Messaging.Authentication.Caching;
using SystemDot.Messaging.Caching;
using SystemDot.Messaging.Expiry;
using SystemDot.Messaging.LoadBalancing;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Pipelines;
using SystemDot.Messaging.Repeating;
using SystemDot.Messaging.Sequencing;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Transport;
using SystemDot.Parallelism;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Publishing.Builders
{
    class SubscriberSendChannelBuilder : ISubscriberSendChannelBuilder
    {
        readonly MessageSender messageSender;
        readonly MessageCacheFactory messageCacheFactory;
        readonly ISystemTime systemTime;
        readonly ITaskRepeater taskRepeater;
        readonly MessageAcknowledgementHandler acknowledgementHandler;
        readonly ISerialiser serialiser;
        readonly ITaskScheduler taskScheduler;
        readonly AuthenticationSessionCache authenticationSessionCache;

        public SubscriberSendChannelBuilder(
            MessageSender messageSender, 
            MessageCacheFactory messageCacheFactory, 
            ISystemTime systemTime, 
            ITaskRepeater taskRepeater, 
            MessageAcknowledgementHandler acknowledgementHandler, 
            ISerialiser serialiser, 
            ITaskScheduler taskScheduler, 
            AuthenticationSessionCache authenticationSessionCache)
        {
            Contract.Requires(messageSender != null);
            Contract.Requires(messageCacheFactory != null);
            Contract.Requires(systemTime != null);
            Contract.Requires(taskRepeater != null);
            Contract.Requires(acknowledgementHandler != null);
            Contract.Requires(serialiser != null);
            Contract.Requires(taskScheduler != null);
            Contract.Requires(authenticationSessionCache != null);

            this.messageSender = messageSender;
            this.messageCacheFactory = messageCacheFactory;
            this.systemTime = systemTime;
            this.taskRepeater = taskRepeater;
            this.acknowledgementHandler = acknowledgementHandler;
            this.serialiser = serialiser;
            this.taskScheduler = taskScheduler;
            this.authenticationSessionCache = authenticationSessionCache;
        }

        public IMessageInputter<MessagePayload> BuildChannel(SubscriberSendChannelSchema schema)
        {
            SendMessageCache cache = CreateCache(schema);
            MessageProcessor startPoint = CreateStartPoint();

            RegisterCacheWithAcknowledgementHandler(cache);
            BuildPipeline(schema, startPoint, cache);
            NotifySubscriberSendChannelBuilt(schema);

            return startPoint;
        }

        SendMessageCache CreateCache(SubscriberSendChannelSchema schema)
        {
            return messageCacheFactory.CreateSendCache(PersistenceUseType.SubscriberSend, schema.SubscriberAddress, schema);
        }

        void RegisterCacheWithAcknowledgementHandler(SendMessageCache cache)
        {
            acknowledgementHandler.RegisterCache(cache);
        }

        MessageProcessor CreateStartPoint()
        {
            return new MessagePayloadCopier(serialiser);
        }

        void BuildPipeline(SubscriberSendChannelSchema schema, MessageProcessor startPoint, SendMessageCache cache)
        {
            MessagePipelineBuilder.Build()
                .With(startPoint)
                .ToProcessor(new AuthenticationSessionAttacher(authenticationSessionCache, schema.SubscriberAddress))
                .ToProcessor(new MessageSendTimeRemover())
                .ToProcessor(new MessageAddresser(schema.FromAddress, schema.SubscriberAddress))
                .ToProcessor(new SendChannelMessageCacher(cache))
                .ToMessageRepeater(cache, systemTime, taskRepeater, schema.RepeatStrategy)
                .ToProcessor(new SendChannelMessageCacheUpdater(cache))
                .ToProcessor(new SequenceOriginRecorder(cache))
                .ToProcessor(new PersistenceSourceRecorder())
                .Queue()
                .ToProcessor(new LoadBalancer(cache, taskScheduler))
                .ToProcessor(new LastSentRecorder(systemTime))
                .ToEndPoint(messageSender);
        }

        static void NotifySubscriberSendChannelBuilt(SubscriberSendChannelSchema schema)
        {
            Messenger.Send(new SubscriberSendChannelBuilt
            {
                CacheAddress = schema.SubscriberAddress,
                SubscriberAddress = schema.SubscriberAddress,
                PublisherAddress = schema.FromAddress
            });
        }
    }
}