using System.Diagnostics.Contracts;
using SystemDot.Messaging.Acknowledgement;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Authentication;
using SystemDot.Messaging.Authentication.Caching;
using SystemDot.Messaging.Authentication.RequestReply;
using SystemDot.Messaging.Batching;
using SystemDot.Messaging.Builders;
using SystemDot.Messaging.Caching;
using SystemDot.Messaging.Correlation;
using SystemDot.Messaging.ExceptionHandling;
using SystemDot.Messaging.Expiry;
using SystemDot.Messaging.Filtering;
using SystemDot.Messaging.Handling;
using SystemDot.Messaging.Hooks;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Pipelines;
using SystemDot.Messaging.Repeating;
using SystemDot.Messaging.RequestReply.ExceptionHandling;
using SystemDot.Messaging.Sequencing;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.ThreadMarshalling;
using SystemDot.Parallelism;
using SystemDot.Serialisation;
using SystemDot.ThreadMashalling;

namespace SystemDot.Messaging.RequestReply.Builders
{
    class RequestRecieveChannelBuilder
    {
        readonly ReplyAddressLookup replyAddressLookup;
        readonly ISerialiser serialiser;
        readonly MessageHandlerRouter messageHandlerRouter;
        readonly AcknowledgementSender acknowledgementSender;
        readonly PersistenceFactorySelector persistenceFactorySelector;
        readonly ISystemTime systemTime;
        readonly ITaskRepeater taskRepeater;
        readonly ServerAddressRegistry serverAddressRegistry;
        readonly IMainThreadMarshaller mainThreadMarshaller;
        readonly AuthenticationSessionCache authenticationSessionCache;
        readonly AuthenticatedServerRegistry authenticatedServerRegistry;
        readonly ReplyAuthenticationSessionLookup replyAuthenticationSessionLookup;
        readonly ReplyCorrelationLookup correlationLookup;

        internal RequestRecieveChannelBuilder(
            ReplyAddressLookup replyAddressLookup,
            ISerialiser serialiser,
            MessageHandlerRouter messageHandlerRouter,
            AcknowledgementSender acknowledgementSender,
            PersistenceFactorySelector persistenceFactorySelector,
            ISystemTime systemTime,
            ITaskRepeater taskRepeater,
            ServerAddressRegistry serverAddressRegistry,
            IMainThreadMarshaller mainThreadMarshaller,
            AuthenticationSessionCache authenticationSessionCache,
            AuthenticatedServerRegistry authenticatedServerRegistry,
            ReplyAuthenticationSessionLookup replyAuthenticationSessionLookup,
            ReplyCorrelationLookup correlationLookup)
        {
            Contract.Requires(replyAddressLookup != null);
            Contract.Requires(serialiser != null);
            Contract.Requires(messageHandlerRouter != null);
            Contract.Requires(acknowledgementSender != null);
            Contract.Requires(persistenceFactorySelector != null);
            Contract.Requires(systemTime != null);
            Contract.Requires(taskRepeater != null);
            Contract.Requires(serverAddressRegistry != null);
            Contract.Requires(mainThreadMarshaller != null);
            Contract.Requires(authenticationSessionCache != null);
            Contract.Requires(authenticatedServerRegistry != null);
            Contract.Requires(replyAuthenticationSessionLookup != null);
            Contract.Requires(correlationLookup != null);

            this.replyAddressLookup = replyAddressLookup;
            this.serialiser = serialiser;
            this.messageHandlerRouter = messageHandlerRouter;
            this.acknowledgementSender = acknowledgementSender;
            this.persistenceFactorySelector = persistenceFactorySelector;
            this.systemTime = systemTime;
            this.taskRepeater = taskRepeater;
            this.serverAddressRegistry = serverAddressRegistry;
            this.mainThreadMarshaller = mainThreadMarshaller;
            this.authenticationSessionCache = authenticationSessionCache;
            this.authenticatedServerRegistry = authenticatedServerRegistry;
            this.replyAuthenticationSessionLookup = replyAuthenticationSessionLookup;
            this.correlationLookup = correlationLookup;
        }

        public IMessageInputter<MessagePayload> Build(RequestRecieveChannelSchema schema, EndpointAddress senderAddress)
        {
            Contract.Requires(schema != null);
            Contract.Requires(senderAddress != null);

            IMessageInputter<MessagePayload> messageInputter = BuildChannel(schema, CreateCache(schema, senderAddress));

            SendChannelBuiltEvent(schema, senderAddress);

            return messageInputter;
        }

        ReceiveMessageCache CreateCache(RequestRecieveChannelSchema schema, EndpointAddress senderAddress)
        {
            return persistenceFactorySelector
                .Select(schema)
                .CreateReceiveCache(PersistenceUseType.RequestReceive, senderAddress);
        }

        IMessageInputter<MessagePayload> BuildChannel(RequestRecieveChannelSchema schema, ReceiveMessageCache messageCache)
        {
            var startPoint = new MessageLocalAddressReassigner(serverAddressRegistry);

            MessagePipelineBuilder.Build()
                .With(startPoint)
                .ToProcessor(new ReceiverAuthenticationSessionVerifier(authenticationSessionCache, authenticatedServerRegistry))
                .ToProcessor(new SequenceOriginApplier(messageCache))
                .ToProcessor(new MessageSendTimeRemover())
                .ToProcessor(new ReceiveChannelMessageCacher(messageCache))
                .ToProcessor(new MessageAcknowledger(acknowledgementSender))
                .ToSimpleMessageRepeater(messageCache, systemTime, taskRepeater)
                .ToProcessor(new ReceiveChannelMessageCacher(messageCache))
                .Queue()
                .ToResequencerIfSequenced(messageCache, schema)
                .ToProcessor(new ReplyChannelSelector(replyAddressLookup, correlationLookup))
                .ToProcessor(new ReplyAuthenticationSessionSelector(replyAuthenticationSessionLookup))
                .ToProcessor(new ExceptionHandler(schema.ContinueOnException))
                .ToProcessor(new ExceptionReplier())
                .ToProcessor(new MessageHookRunner<MessagePayload>(schema.PreUnpackagingHooks))
                .ToConverter(new MessagePayloadUnpackager(serialiser))
                .ToProcessor(new MessageFilter(schema.FilterStrategy))
                .ToProcessor(schema.UnitOfWorkRunnerCreator())
                .ToProcessor(new BatchUnpackager())
                .ToProcessorIf(new MainThreadMessageMashaller(mainThreadMarshaller), schema.HandleRequestsOnMainThread)
                .ToProcessor(new MessageHookRunner<object>(schema.Hooks))
                .ToEndPoint(messageHandlerRouter);
            return startPoint;
        }

        static void SendChannelBuiltEvent(RequestRecieveChannelSchema schema, EndpointAddress senderAddress)
        {
            Messenger.Send(new RequestReceiveChannelBuilt
            {
                CacheAddress = senderAddress,
                SenderAddress = senderAddress,
                ReceiverAddress = schema.Address
            });
        }
    }
}