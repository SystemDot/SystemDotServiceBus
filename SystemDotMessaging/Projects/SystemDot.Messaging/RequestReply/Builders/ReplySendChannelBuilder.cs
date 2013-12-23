using System.Diagnostics.Contracts;
using SystemDot.Messaging.Acknowledgement;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Authentication;
using SystemDot.Messaging.Authentication.Caching;
using SystemDot.Messaging.Authentication.RequestReply;
using SystemDot.Messaging.Batching;
using SystemDot.Messaging.Caching;
using SystemDot.Messaging.Correlation;
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
        readonly MessageCacheFactory messageCacheFactory;
        readonly MessageAcknowledgementHandler acknowledgementHandler;
        readonly ITaskScheduler taskScheduler;
        readonly ReplyAuthenticationSessionAttacherFactory authenticationSessionAttacherFactory;
        readonly ReplyCorrelationLookup correlationLookup;

        public ReplySendChannelBuilder(
            MessageSender messageSender,
            ISerialiser serialiser,
            ISystemTime systemTime,
            ITaskRepeater taskRepeater,
            MessageCacheFactory messageCacheFactory,
            MessageAcknowledgementHandler acknowledgementHandler,
            ITaskScheduler taskScheduler,
            ReplyAuthenticationSessionAttacherFactory authenticationSessionAttacherFactory,
            ReplyCorrelationLookup correlationLookup)
        {
            Contract.Requires(messageSender != null);
            Contract.Requires(serialiser != null);
            Contract.Requires(systemTime != null);
            Contract.Requires(taskRepeater != null);
            Contract.Requires(messageCacheFactory != null);
            Contract.Requires(acknowledgementHandler != null);
            Contract.Requires(taskScheduler != null);
            Contract.Requires(authenticationSessionAttacherFactory != null);
            Contract.Requires(correlationLookup != null);

            this.messageSender = messageSender;
            this.serialiser = serialiser;
            this.systemTime = systemTime;
            this.taskRepeater = taskRepeater;
            this.messageCacheFactory = messageCacheFactory;
            this.acknowledgementHandler = acknowledgementHandler;
            this.taskScheduler = taskScheduler;
            this.authenticationSessionAttacherFactory = authenticationSessionAttacherFactory;
            this.correlationLookup = correlationLookup;
        }

        public IMessageInputter<object> Build(ReplySendChannelSchema schema, EndpointAddress senderAddress)
        {
            SendMessageCache cache = CreateCache(schema, senderAddress);
            RegisterCacheWithAcknowledgementHandler(cache);

            IMessageProcessor<object, object> startPoint = CreateStartPoint();
            BuildPipeline(startPoint, schema, senderAddress, cache);

            SendChannelBuiltEvent(schema, senderAddress);

            return startPoint;
        }

        SendMessageCache CreateCache(ReplySendChannelSchema schema, EndpointAddress senderAddress)
        {
            return messageCacheFactory.BuildSendCache(PersistenceUseType.ReplySend, senderAddress, schema);
        }

        void RegisterCacheWithAcknowledgementHandler(SendMessageCache cache)
        {
            acknowledgementHandler.RegisterCache(cache);
        }

        static IMessageProcessor<object, object> CreateStartPoint()
        {
            return new Pipe<object>();
        }

        void BuildPipeline(IMessageProcessor<object, object> startPoint, ReplySendChannelSchema schema, EndpointAddress senderAddress, SendMessageCache cache)
        {
            MessagePipelineBuilder.Build()
                .With(startPoint)
                .ToProcessor(new MessageHookRunner<object>(schema.Hooks))
                .ToProcessor(new BatchPackager())
                .ToConverter(new MessagePayloadPackager(serialiser))
                .ToProcessor(new ReplyCorrelationApplier(correlationLookup))
                .ToProcessor(new Sequencer(cache))
                .ToProcessor(new MessageAddresser(schema.FromAddress, senderAddress))
                .ToProcessor(new SendChannelMessageCacher(cache))
                .ToMessageRepeater(cache, systemTime, taskRepeater, schema.RepeatStrategy)
                .ToProcessor(new SendChannelMessageCacheUpdater(cache))
                .ToProcessor(new SequenceOriginRecorder(cache))
                .ToProcessor(new PersistenceSourceRecorder())
                .Queue()
                .ToProcessor(new MessageExpirer(schema.ExpiryAction, cache, schema.ExpiryStrategy))
                .ToProcessor(new LoadBalancer(cache, taskScheduler))
                .ToProcessor(new LastSentRecorder(systemTime))
                .ToProcessor(authenticationSessionAttacherFactory.Create())
                .ToEndPoint(messageSender);
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