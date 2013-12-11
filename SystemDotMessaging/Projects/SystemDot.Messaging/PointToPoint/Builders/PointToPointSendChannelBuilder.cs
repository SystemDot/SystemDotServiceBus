using System.Diagnostics.Contracts;
using SystemDot.Messaging.Acknowledgement;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Authentication;
using SystemDot.Messaging.Authentication.Caching;
using SystemDot.Messaging.Authentication.Expiry;
using SystemDot.Messaging.Batching;
using SystemDot.Messaging.Builders;
using SystemDot.Messaging.Caching;
using SystemDot.Messaging.Expiry;
using SystemDot.Messaging.Filtering;
using SystemDot.Messaging.LoadBalancing;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Pipelines;
using SystemDot.Messaging.Repeating;
using SystemDot.Messaging.Sequencing;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Transport;
using SystemDot.Parallelism;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.PointToPoint.Builders
{
    class PointToPointSendChannelBuilder
    {
        readonly MessageSender messageSender;
        readonly ISerialiser serialiser;
        readonly ISystemTime systemTime;
        readonly ITaskRepeater taskRepeater;
        readonly MessageCacheFactory messageCacheFactory;
        readonly MessageAcknowledgementHandler acknowledgementHandler;
        readonly ITaskScheduler taskScheduler;
        readonly AuthenticationSessionCache authenticationSessionCache;
        readonly AuthenticatedServerRegistry authenticatedServerRegistry;

        public PointToPointSendChannelBuilder(
            MessageSender messageSender, 
            ISerialiser serialiser, 
            ISystemTime systemTime, 
            ITaskRepeater taskRepeater, 
            MessageCacheFactory messageCacheFactory, 
            MessageAcknowledgementHandler acknowledgementHandler, 
            ITaskScheduler taskScheduler, 
            AuthenticationSessionCache authenticationSessionCache, 
            AuthenticatedServerRegistry authenticatedServerRegistry)
        {
            Contract.Requires(messageSender != null);
            Contract.Requires(serialiser != null);
            Contract.Requires(systemTime != null);
            Contract.Requires(taskRepeater != null);
            Contract.Requires(messageCacheFactory != null);
            Contract.Requires(acknowledgementHandler != null);
            Contract.Requires(taskScheduler != null);
            Contract.Requires(authenticationSessionCache != null);
            Contract.Requires(authenticatedServerRegistry != null);

            this.messageSender = messageSender;
            this.serialiser = serialiser;
            this.systemTime = systemTime;
            this.taskRepeater = taskRepeater;
            this.messageCacheFactory = messageCacheFactory;
            this.acknowledgementHandler = acknowledgementHandler;
            this.taskScheduler = taskScheduler;
            this.authenticationSessionCache = authenticationSessionCache;
            this.authenticatedServerRegistry = authenticatedServerRegistry;
        }

        public void Build(PointToPointSendChannelSchema schema)
        {
            Contract.Requires(schema != null);

            SendMessageCache cache = CreateCache(schema);
            RegisterCacheWithAcknowledgementHandler(cache);
            BuildPipeline(schema, cache);
            NotifyPointToPointSendChannelBuilt(schema);
        }

        SendMessageCache CreateCache(PointToPointSendChannelSchema schema)
        {
            return messageCacheFactory.BuildSendCache(PersistenceUseType.PointToPointSend, schema.FromAddress, schema);
        }

        void RegisterCacheWithAcknowledgementHandler(SendMessageCache cache)
        {
            acknowledgementHandler.RegisterCache(cache);
        }

        void BuildPipeline(PointToPointSendChannelSchema schema, SendMessageCache cache)
        {
            MessagePipelineBuilder.Build()
                .WithBusSendTo(new MessageFilter(schema.FilteringStrategy))
                .ToProcessor(new BatchPackager())
                .ToConverter(new MessagePayloadPackager(serialiser))
                .ToProcessor(new AuthenticationSessionAttacher(authenticationSessionCache, schema.ReceiverAddress))
                .ToProcessor(new Sequencer(cache))
                .ToProcessor(new MessageAddresser(schema.FromAddress, schema.ReceiverAddress))
                .ToProcessor(new SendChannelMessageCacher(cache))
                .ToMessageRepeater(cache, systemTime, taskRepeater, schema.RepeatStrategy)
                .ToProcessor(new SendChannelMessageCacheUpdater(cache))
                .ToProcessor(new SequenceOriginRecorder(cache))
                .ToProcessor(new PersistenceSourceRecorder())
                .Queue()
                .ToProcessor(new MessageExpirer(schema.ExpiryAction, cache, schema.ExpiryStrategy, CreateSessionExpiryStrategy(schema)))
                .ToProcessor(new LoadBalancer(cache, taskScheduler))
                .ToProcessor(new LastSentRecorder(systemTime))
                .ToEndPoint(messageSender);
        }

        AuthenticationSessionExpiryStrategy CreateSessionExpiryStrategy(PointToPointSendChannelSchema schema)
        {
            return new AuthenticationSessionExpiryStrategy(authenticatedServerRegistry, schema.ReceiverAddress.Server, systemTime);
        }

        static void NotifyPointToPointSendChannelBuilt(PointToPointSendChannelSchema schema)
        {
            Messenger.Send(new PointToPointSendChannelBuilt
            {
                SenderAddress = schema.FromAddress,
                ReceiverAddress = schema.ReceiverAddress
            });
        }
    }
}