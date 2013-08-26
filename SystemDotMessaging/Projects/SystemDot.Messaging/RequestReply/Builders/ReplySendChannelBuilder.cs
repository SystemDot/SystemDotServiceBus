using System.Diagnostics.Contracts;
using SystemDot.Messaging.Acknowledgement;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Authentication;
using SystemDot.Messaging.Authentication.Caching;
using SystemDot.Messaging.Authentication.Expiry;
using SystemDot.Messaging.Authentication.RequestReply;
using SystemDot.Messaging.Batching;
using SystemDot.Messaging.Builders;
using SystemDot.Messaging.Caching;
using SystemDot.Messaging.Distribution;
using SystemDot.Messaging.Expiry;
using SystemDot.Messaging.Hooks;
using SystemDot.Messaging.LoadBalancing;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Pipelines;
using SystemDot.Messaging.Repeating;
using SystemDot.Messaging.Sequencing;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Transport;
using SystemDot.Parallelism;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.RequestReply.Builders
{
    class ReplySendChannelBuilder
    {
        readonly MessageSender messageSender;
        readonly ISerialiser serialiser;
        readonly ISystemTime systemTime;
        readonly ITaskRepeater taskRepeater;
        readonly PersistenceFactorySelector persistenceFactorySelector;
        readonly MessageAcknowledgementHandler acknowledgementHandler;
        readonly ITaskScheduler taskScheduler;
        readonly AuthenticationSessionCache authenticationSessionCache;
        readonly ReplyAuthenticationSessionLookup replyAuthenticationSessionLookup;
        readonly AuthenticatedServerRegistry authenticatedServerRegistry;

        public ReplySendChannelBuilder(
            MessageSender messageSender, 
            ISerialiser serialiser, 
            ISystemTime systemTime, 
            ITaskRepeater taskRepeater, 
            PersistenceFactorySelector persistenceFactorySelector, 
            MessageAcknowledgementHandler acknowledgementHandler, 
            ITaskScheduler taskScheduler, 
            AuthenticationSessionCache authenticationSessionCache, 
            ReplyAuthenticationSessionLookup replyAuthenticationSessionLookup, 
            AuthenticatedServerRegistry authenticatedServerRegistry)
        {
            Contract.Requires(messageSender != null);
            Contract.Requires(serialiser != null);
            Contract.Requires(systemTime != null);
            Contract.Requires(taskRepeater != null);
            Contract.Requires(persistenceFactorySelector != null);
            Contract.Requires(acknowledgementHandler != null);
            Contract.Requires(taskScheduler != null);
            Contract.Requires(authenticationSessionCache != null);
            Contract.Requires(replyAuthenticationSessionLookup != null);
            Contract.Requires(authenticatedServerRegistry != null);
            
            this.messageSender = messageSender;
            this.serialiser = serialiser;
            this.systemTime = systemTime;
            this.taskRepeater = taskRepeater;
            this.persistenceFactorySelector = persistenceFactorySelector;
            this.acknowledgementHandler = acknowledgementHandler;
            this.taskScheduler = taskScheduler;
            this.authenticationSessionCache = authenticationSessionCache;
            this.replyAuthenticationSessionLookup = replyAuthenticationSessionLookup;
            this.authenticatedServerRegistry = authenticatedServerRegistry;
        }

        public IMessageInputter<object> Build(ReplySendChannelSchema schema, EndpointAddress senderAddress)
        {
            SendMessageCache cache = CreateCache(schema, senderAddress);

            RegisterCacheWithAcknowledgementHandler(cache);

            Pipe<object> startPoint = BuildPipeline(schema, senderAddress, cache);

            SendChannelBuiltEvent(schema, senderAddress);

            return startPoint;
        }

        void RegisterCacheWithAcknowledgementHandler(SendMessageCache cache)
        {
            acknowledgementHandler.RegisterCache(cache);
        }

        Pipe<object> BuildPipeline(ReplySendChannelSchema schema, EndpointAddress senderAddress, SendMessageCache cache)
        {
            var startPoint = new Pipe<object>();

            MessagePipelineBuilder.Build()
                .With(startPoint)
                .ToProcessor(new MessageHookRunner<object>(schema.Hooks))
                .ToProcessor(new BatchPackager())
                .ToConverter(new MessagePayloadPackager(serialiser))
                .ToProcessor(new ReplyAuthenticationSessionAttacher(replyAuthenticationSessionLookup))
                .ToProcessor(new Sequencer(cache))
                .ToProcessor(new MessageAddresser(schema.FromAddress, senderAddress))
                .ToProcessor(new SendChannelMessageCacher(cache))
                .ToMessageRepeater(cache, systemTime, taskRepeater, schema.RepeatStrategy)
                .ToProcessor(new SendChannelMessageCacheUpdater(cache))
                .ToProcessor(new SequenceOriginRecorder(cache))
                .ToProcessor(new PersistenceSourceRecorder())
                .Queue()
                .ToProcessor(new MessageExpirer(schema.ExpiryAction, cache, schema.ExpiryStrategy, CreateAuthenticationSessionExpiryStrategy(schema)))
                .ToProcessor(new LoadBalancer(cache, taskScheduler))
                .ToProcessor(new LastSentRecorder(systemTime))
                .ToEndPoint(messageSender);

            return startPoint;
        }

        SendMessageCache CreateCache(ReplySendChannelSchema schema, EndpointAddress senderAddress)
        {
            return persistenceFactorySelector
                .Select(schema)
                .CreateSendCache(PersistenceUseType.ReplySend, senderAddress);
        }

        AuthenticationSessionExpiryStrategy CreateAuthenticationSessionExpiryStrategy(ReplySendChannelSchema schema)
        {
            return new AuthenticationSessionExpiryStrategy(
                authenticatedServerRegistry, 
                schema.FromAddress.Server, 
                systemTime);
        }

        static void SendChannelBuiltEvent(ReplySendChannelSchema schema, EndpointAddress senderAddress)
        {
            Messenger.Send(new ReplySendChannelBuilt
            {
                CacheAddress = senderAddress,
                ReceiverAddress = schema.FromAddress,
                SenderAddress = senderAddress
            });
        }
    }
}