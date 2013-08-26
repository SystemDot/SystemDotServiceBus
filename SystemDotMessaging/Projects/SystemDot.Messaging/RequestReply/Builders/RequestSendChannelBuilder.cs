using System.Diagnostics.Contracts;
using SystemDot.Messaging.Acknowledgement;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Authentication;
using SystemDot.Messaging.Authentication.Caching;
using SystemDot.Messaging.Batching;
using SystemDot.Messaging.Builders;
using SystemDot.Messaging.Caching;
using SystemDot.Messaging.Expiry;
using SystemDot.Messaging.Filtering;
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
    class RequestSendChannelBuilder
    {
        readonly MessageSender messageSender;
        readonly ISerialiser serialiser;
        readonly ISystemTime systemTime;
        readonly ITaskRepeater taskRepeater;
        readonly PersistenceFactorySelector persistenceFactory;
        readonly MessageAcknowledgementHandler acknowledgementHandler;
        readonly ITaskScheduler taskScheduler;
        readonly AuthenticationSessionCache authenticationSessionCache;

        public RequestSendChannelBuilder(
            MessageSender messageSender, 
            ISerialiser serialiser, 
            ISystemTime systemTime, 
            ITaskRepeater taskRepeater, 
            PersistenceFactorySelector persistenceFactory, 
            MessageAcknowledgementHandler acknowledgementHandler, 
            ITaskScheduler taskScheduler, 
            AuthenticationSessionCache authenticationSessionCache)
        {
            Contract.Requires(messageSender != null);
            Contract.Requires(serialiser != null);
            Contract.Requires(systemTime != null);
            Contract.Requires(taskRepeater != null);
            Contract.Requires(persistenceFactory != null);
            Contract.Requires(acknowledgementHandler != null);
            Contract.Requires(taskScheduler != null);
            Contract.Requires(authenticationSessionCache != null);
            
            this.messageSender = messageSender;
            this.serialiser = serialiser;
            this.systemTime = systemTime;
            this.taskRepeater = taskRepeater;
            this.persistenceFactory = persistenceFactory;
            this.acknowledgementHandler = acknowledgementHandler;
            this.taskScheduler = taskScheduler;
            this.authenticationSessionCache = authenticationSessionCache;
        }

        public void Build(RequestSendChannelSchema schema)
        {
            SendMessageCache cache = persistenceFactory
                .Select(schema)
                .CreateSendCache(PersistenceUseType.RequestSend, schema.FromAddress);

            acknowledgementHandler.RegisterCache(cache);

            MessagePipelineBuilder.Build()
                .WithBusSendTo(new MessageFilter(schema.FilteringStrategy))
                .ToProcessor(new MessageHookRunner<object>(schema.Hooks))
                .ToProcessor(new BatchPackager())
                .ToConverter(new MessagePayloadPackager(serialiser))
                .ToProcessor(new AuthenticationSessionAttacher(authenticationSessionCache, schema.ReceiverAddress))
                .ToProcessor(new MessageHookRunner<MessagePayload>(schema.PostPackagingHooks))
                .ToProcessor(new Sequencer(cache))
                .ToProcessor(new MessageAddresser(schema.FromAddress, schema.ReceiverAddress))
                .ToProcessor(new SendChannelMessageCacher(cache))
                .ToMessageRepeater(cache, systemTime, taskRepeater, schema.RepeatStrategy)
                .ToProcessor(new SendChannelMessageCacheUpdater(cache))
                .ToProcessor(new SequenceOriginRecorder(cache))
                .ToProcessor(new PersistenceSourceRecorder())
                .Queue()
                .ToProcessor(new MessageExpirer(schema.ExpiryAction, cache, schema.ExpiryStrategy))
                .ToProcessor(new LoadBalancer(cache, taskScheduler))
                .ToProcessor(new LastSentRecorder(systemTime))
                .ToEndPoint(messageSender);

            Messenger.Send(new RequestSendChannelBuilt
            {
                CacheAddress = schema.FromAddress, 
                SenderAddress = schema.FromAddress, 
                ReceiverAddress = schema.ReceiverAddress
            });
        }
    }
}