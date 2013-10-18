using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Acknowledgement;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Authentication;
using SystemDot.Messaging.Authentication.Caching;
using SystemDot.Messaging.Builders;
using SystemDot.Messaging.Caching;
using SystemDot.Messaging.ExceptionHandling;
using SystemDot.Messaging.Expiry;
using SystemDot.Messaging.Filtering;
using SystemDot.Messaging.Handling;
using SystemDot.Messaging.Hooks;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Pipelines;
using SystemDot.Messaging.Repeating;
using SystemDot.Messaging.Sequencing;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.ThreadMarshalling;
using SystemDot.Messaging.Transport;
using SystemDot.Parallelism;
using SystemDot.Serialisation;
using SystemDot.ThreadMashalling;

namespace SystemDot.Messaging.Publishing.Builders
{
    class SubscriberRecieveChannelBuilder
    {
        readonly ISerialiser serialiser;
        readonly MessageHandlerRouter messageHandlerRouter;
        readonly MessageReceiver messageReceiver;
        readonly AcknowledgementSender acknowledgementSender;
        readonly PersistenceFactorySelector persistenceFactory;
        readonly ISystemTime systemTime;
        readonly ITaskRepeater taskRepeater;
        readonly ServerAddressRegistry serverAddressRegistry;
        readonly IMainThreadMarshaller mainThreadMarshaller;
        readonly AuthenticationSessionCache authenticationSessionCache;
        readonly AuthenticatedServerRegistry authenticatedServerRegistry;

        internal SubscriberRecieveChannelBuilder(
            ISerialiser serialiser, 
            MessageHandlerRouter messageHandlerRouter, 
            MessageReceiver messageReceiver,
            AcknowledgementSender acknowledgementSender,
            PersistenceFactorySelector persistenceFactory, 
            ISystemTime systemTime, 
            ITaskRepeater taskRepeater, 
            ServerAddressRegistry serverAddressRegistry, 
            IMainThreadMarshaller mainThreadMarshaller, 
            AuthenticationSessionCache authenticationSessionCache, 
            AuthenticatedServerRegistry authenticatedServerRegistry)
        {
            Contract.Requires(serialiser != null);
            Contract.Requires(messageHandlerRouter != null);
            Contract.Requires(messageReceiver != null);
            Contract.Requires(acknowledgementSender != null);
            Contract.Requires(persistenceFactory != null);
            Contract.Requires(systemTime != null);
            Contract.Requires(taskRepeater != null);
            Contract.Requires(serverAddressRegistry != null);
            Contract.Requires(mainThreadMarshaller != null);
            Contract.Requires(authenticationSessionCache != null);
            Contract.Requires(authenticatedServerRegistry != null);
            
            this.serialiser = serialiser;
            this.messageHandlerRouter = messageHandlerRouter;
            this.messageReceiver = messageReceiver;
            this.acknowledgementSender = acknowledgementSender;
            this.persistenceFactory = persistenceFactory;
            this.systemTime = systemTime;
            this.taskRepeater = taskRepeater;
            this.serverAddressRegistry = serverAddressRegistry;
            this.mainThreadMarshaller = mainThreadMarshaller;
            this.authenticationSessionCache = authenticationSessionCache;
            this.authenticatedServerRegistry = authenticatedServerRegistry;
        }

        public void Build(SubscriberRecieveChannelSchema schema)
        {
            ReceiveMessageCache messageCache = persistenceFactory
                .Select(schema)
                    .CreateReceiveCache(PersistenceUseType.SubscriberReceive, schema.Address);

            MessagePipelineBuilder.Build()
                .With(messageReceiver)
                .ToProcessor(new BodyMessageFilter(schema.Address))
                .ToProcessor(new SenderAuthenticationSessionVerifier(authenticationSessionCache, authenticatedServerRegistry))
                .ToProcessor(new MessageLocalAddressReassigner(serverAddressRegistry))
                .ToProcessor(new MessageSendTimeRemover())
                .ToProcessor(new SequenceOriginApplier(messageCache))
                .ToProcessor(new ReceiveChannelMessageCacher(messageCache))
                .ToProcessor(new MessageAcknowledger(acknowledgementSender))
                .ToSimpleMessageRepeater(messageCache, systemTime, taskRepeater)
                .ToProcessor(new ReceiveChannelMessageCacher(messageCache))
                .Queue()
                .ToResequencerIfSequenced(messageCache, schema)
                .ToProcessor(new ExceptionHandler(schema.ContinueOnException))
                .ToProcessor(new MessageHookRunner<MessagePayload>(schema.PreUnpackagingHooks))
                .ToConverter(new MessagePayloadUnpackager(serialiser))
                .ToProcessor(new MessageFilter(schema.FilterStrategy))
                .ToProcessorIf(new MainThreadMessageMashaller(mainThreadMarshaller), schema.HandleEventsOnMainThread)
                .ToProcessor(schema.UnitOfWorkRunnerCreator())
                .ToProcessor(new MessageHookRunner<object>(schema.Hooks))
                .ToEndPoint(messageHandlerRouter);

            Messenger.Send(new SubscriberReceiveChannelBuilt
            {
                CacheAddress = schema.Address,
                SubscriberAddress = schema.Address,
                PublisherAddress = schema.ToAddress
            });
        }
    }

    
}