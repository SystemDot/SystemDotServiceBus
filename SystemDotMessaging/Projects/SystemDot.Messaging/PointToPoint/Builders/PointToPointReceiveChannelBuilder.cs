using System;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Acknowledgement;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Authentication;
using SystemDot.Messaging.Authentication.Caching;
using SystemDot.Messaging.Batching;
using SystemDot.Messaging.Caching;
using SystemDot.Messaging.ExceptionHandling;
using SystemDot.Messaging.Expiry;
using SystemDot.Messaging.Filtering;
using SystemDot.Messaging.Handling;
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
    class PointToPointReceiveChannelBuilder
    {
        readonly MessageReceiver messageReceiver;
        readonly ISerialiser serialiser;
        readonly AcknowledgementSender acknowledgementSender;
        readonly MessageHandlerRouter messageHandlerRouter;
        readonly MessageCacheFactory messageCacheFactory;
        readonly ISystemTime systemTime;
        readonly ITaskRepeater taskRepeater;
        readonly ServerAddressRegistry serverAddressRegistry;
        readonly AuthenticationSessionCache authenticationSessionCache;
        readonly AuthenticatedServerRegistry authenticatedServerRegistry;

        internal PointToPointReceiveChannelBuilder(
            MessageReceiver messageReceiver, 
            ISerialiser serialiser, 
            AcknowledgementSender acknowledgementSender, 
            MessageHandlerRouter messageHandlerRouter,
            MessageCacheFactory messageCacheFactory, 
            ISystemTime systemTime, 
            ITaskRepeater taskRepeater, 
            ServerAddressRegistry serverAddressRegistry, 
            AuthenticationSessionCache authenticationSessionCache, 
            AuthenticatedServerRegistry authenticatedServerRegistry)
        {
            Contract.Requires(messageReceiver != null);
            Contract.Requires(serialiser != null);
            Contract.Requires(acknowledgementSender != null);
            Contract.Requires(messageHandlerRouter != null);
            Contract.Requires(messageCacheFactory != null);
            Contract.Requires(systemTime != null);
            Contract.Requires(taskRepeater != null);
            Contract.Requires(serverAddressRegistry != null);
            Contract.Requires(authenticationSessionCache != null);
            Contract.Requires(authenticatedServerRegistry != null);

            this.messageReceiver = messageReceiver;
            this.serialiser = serialiser;
            this.acknowledgementSender = acknowledgementSender;
            this.messageHandlerRouter = messageHandlerRouter;
            this.messageCacheFactory = messageCacheFactory;
            this.systemTime = systemTime;
            this.taskRepeater = taskRepeater;
            this.serverAddressRegistry = serverAddressRegistry;
            this.authenticationSessionCache = authenticationSessionCache;
            this.authenticatedServerRegistry = authenticatedServerRegistry;
        }

        public void Build(PointToPointReceiverChannelSchema schema)
        {
            BuildPipeline(schema, CreateCache(schema));
            NotifyPointToPointReceiveChannelBuilt(schema);
        }

        ReceiveMessageCache CreateCache(PointToPointReceiverChannelSchema schema)
        {
            return messageCacheFactory.CreateReceiveCache(PersistenceUseType.PointToPointReceive, schema.Address, schema);
        }

        void BuildPipeline(PointToPointReceiverChannelSchema schema, ReceiveMessageCache messageCache)
        {
            MessagePipelineBuilder.Build()
                .With(messageReceiver)
                .ToProcessor(new BodyMessageFilter(schema.Address))
                .ToProcessorIf(new NullMessageProcessor(), schema.FlushMessages)
                .ToProcessor(new ReceiverAuthenticationSessionVerifier(authenticationSessionCache, authenticatedServerRegistry))
                .ToProcessor(new MessageLocalAddressReassigner(serverAddressRegistry))
                .ToProcessor(new SequenceOriginApplier(messageCache))
                .ToProcessor(new MessageSendTimeRemover())
                .ToProcessor(new ReceiveChannelMessageCacher(messageCache))
                .ToProcessor(new MessageAcknowledger(acknowledgementSender))
                .ToSimpleMessageRepeater(messageCache, systemTime, taskRepeater)
                .ToProcessor(new ReceiveChannelMessageCacher(messageCache))
                .Queue()
                .ToResequencerIfSequenced(messageCache, schema)
                .ToProcessor(new ExceptionHandler(schema.ContinueOnException))
                .ToConverter(new MessagePayloadUnpackager(serialiser))
                .ToProcessor(new MessageFilter(schema.FilterStrategy))
                .ToProcessor(schema.UnitOfWorkRunnerCreator())
                .ToProcessor(new BatchUnpackager())
                .ToEndPoint(messageHandlerRouter);
        }

        static void NotifyPointToPointReceiveChannelBuilt(PointToPointReceiverChannelSchema schema)
        {
            Messenger.Send(new PointToPointReceiveChannelBuilt {Address = schema.Address});
        }
    }
}