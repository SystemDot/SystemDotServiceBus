using System.Diagnostics.Contracts;
using SystemDot.Messaging.Acknowledgement;
using SystemDot.Messaging.Builders;
using SystemDot.Messaging.Caching;
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

namespace SystemDot.Messaging.Publishing.Builders
{
    class SubscriberRecieveChannelBuilder
    {
        readonly ISerialiser serialiser;
        readonly MessageHandlerRouter messageHandlerRouter;
        readonly IMessageReceiver messageReceiver;
        readonly AcknowledgementSender acknowledgementSender;
        readonly PersistenceFactorySelector persistenceFactory;
        readonly ISystemTime systemTime;
        readonly ITaskRepeater taskRepeater;

        internal SubscriberRecieveChannelBuilder(
            ISerialiser serialiser, 
            MessageHandlerRouter messageHandlerRouter, 
            IMessageReceiver messageReceiver,
            AcknowledgementSender acknowledgementSender,
            PersistenceFactorySelector persistenceFactory, 
            ISystemTime systemTime, 
            ITaskRepeater taskRepeater)
        {
            Contract.Requires(serialiser != null);
            Contract.Requires(messageHandlerRouter != null);
            Contract.Requires(messageReceiver != null);
            Contract.Requires(acknowledgementSender != null);
            Contract.Requires(persistenceFactory != null);
            Contract.Requires(systemTime != null);
            Contract.Requires(taskRepeater != null);
            
            this.serialiser = serialiser;
            this.messageHandlerRouter = messageHandlerRouter;
            this.messageReceiver = messageReceiver;
            this.acknowledgementSender = acknowledgementSender;
            this.persistenceFactory = persistenceFactory;
            this.systemTime = systemTime;
            this.taskRepeater = taskRepeater;
        }

        public void Build(SubscriberRecieveChannelSchema schema)
        {
            ReceiveMessageCache messageCache = persistenceFactory
                .Select(schema)
                    .CreateReceiveCache(PersistenceUseType.SubscriberReceive, schema.Address);

            MessagePipelineBuilder.Build()
                .With(messageReceiver)
                .ToProcessor(new BodyMessageFilter(schema.Address))
                .ToProcessor(new MessageSendTimeRemover())
                .ToProcessor(new SequenceOriginApplier(messageCache))
                .ToProcessor(new ReceiveChannelMessageCacher(messageCache))
                .ToProcessor(new MessageAcknowledger(acknowledgementSender))
                .ToSimpleMessageRepeater(messageCache, systemTime, taskRepeater)
                .ToProcessor(new ReceiveChannelMessageCacher(messageCache))
                .Queue()
                .ToResequencerIfSequenced(messageCache, schema)
                .ToProcessors(schema.PreUnpackagingHooks.ToArray())
                .ToConverter(new MessagePayloadUnpackager(serialiser))
                .ToProcessor(new MessageFilter(schema.FilterStrategy))
                .ToProcessor(schema.UnitOfWorkRunnerCreator())
                .ToProcessors(schema.Hooks.ToArray())
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