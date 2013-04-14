using System.Diagnostics.Contracts;
using SystemDot.Messaging.Acknowledgement;
using SystemDot.Messaging.Batching;
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

namespace SystemDot.Messaging.PointToPoint.Builders
{
    class PointToPointReceiveChannelBuilder
    {
        readonly IMessageReceiver messageReceiver;
        readonly ISerialiser serialiser;
        readonly AcknowledgementSender acknowledgementSender;
        readonly MessageHandlerRouter messageHandlerRouter;
        readonly PersistenceFactorySelector persistenceFactorySelector;
        readonly ISystemTime systemTime;
        readonly ITaskRepeater taskRepeater;

        internal PointToPointReceiveChannelBuilder(
            IMessageReceiver messageReceiver, 
            ISerialiser serialiser, 
            AcknowledgementSender acknowledgementSender, 
            MessageHandlerRouter messageHandlerRouter,
            PersistenceFactorySelector persistenceFactorySelector, 
            ISystemTime systemTime, 
            ITaskRepeater taskRepeater)
        {
            Contract.Requires(messageReceiver != null);
            Contract.Requires(serialiser != null);
            Contract.Requires(acknowledgementSender != null);
            Contract.Requires(messageHandlerRouter != null);
            Contract.Requires(persistenceFactorySelector != null);
            Contract.Requires(systemTime != null);
            Contract.Requires(taskRepeater != null);

            this.messageReceiver = messageReceiver;
            this.serialiser = serialiser;
            this.acknowledgementSender = acknowledgementSender;
            this.messageHandlerRouter = messageHandlerRouter;
            this.persistenceFactorySelector = persistenceFactorySelector;
            this.systemTime = systemTime;
            this.taskRepeater = taskRepeater;
        }

        public void Build(PointToPointReceiverChannelSchema schema)
        {
            ReceiveMessageCache messageCache = this.persistenceFactorySelector
                .Select(schema)
                .CreateReceiveCache(PersistenceUseType.PointToPointReceive, schema.Address);

            MessagePipelineBuilder.Build()
                .With(this.messageReceiver)
                .ToProcessor(new BodyMessageFilter(schema.Address))
                .ToProcessor(new SequenceOriginApplier(messageCache))
                .ToProcessor(new MessageSendTimeRemover())
                .ToProcessor(new ReceiveChannelMessageCacher(messageCache))
                .ToProcessor(new MessageAcknowledger(this.acknowledgementSender))
                .ToSimpleMessageRepeater(messageCache, this.systemTime, this.taskRepeater)
                .ToProcessor(new ReceiveChannelMessageCacher(messageCache))
                .Queue()
                .ToResequencerIfSequenced(messageCache, schema)
                .ToConverter(new MessagePayloadUnpackager(this.serialiser))
                .ToProcessor(schema.UnitOfWorkRunnerCreator())
                .ToProcessor(new BatchUnpackager())
                .ToEndPoint(this.messageHandlerRouter);

            Messenger.Send(new PointToPointReceiveChannelBuilt { Address = schema.Address });
        }
    }
}