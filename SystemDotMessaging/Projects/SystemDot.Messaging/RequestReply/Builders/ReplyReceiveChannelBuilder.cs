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

namespace SystemDot.Messaging.RequestReply.Builders
{
    class ReplyReceiveChannelBuilder
    {
        readonly ISerialiser serialiser;
        readonly MessageHandlerRouter messageHandlerRouter;
        readonly IMessageReceiver messageReceiver;
        readonly AcknowledgementSender acknowledgementSender;
        readonly PersistenceFactorySelector persistenceFactorySelector;
        readonly ISystemTime systemTime;
        readonly ITaskRepeater taskRepeater;

        internal ReplyReceiveChannelBuilder(
            ISerialiser serialiser, 
            MessageHandlerRouter messageHandlerRouter, 
            IMessageReceiver messageReceiver, 
            AcknowledgementSender acknowledgementSender,
            PersistenceFactorySelector persistenceFactorySelector, 
            ISystemTime systemTime, 
            ITaskRepeater taskRepeater)
        {
            Contract.Requires(serialiser != null);
            Contract.Requires(messageHandlerRouter != null);
            Contract.Requires(messageReceiver != null);
            Contract.Requires(acknowledgementSender != null);
            Contract.Requires(persistenceFactorySelector != null);
            Contract.Requires(systemTime != null);
            Contract.Requires(taskRepeater != null);
            
            this.serialiser = serialiser;
            this.messageHandlerRouter = messageHandlerRouter;
            this.messageReceiver = messageReceiver;
            this.acknowledgementSender = acknowledgementSender;
            this.persistenceFactorySelector = persistenceFactorySelector;
            this.systemTime = systemTime;
            this.taskRepeater = taskRepeater;
        }

        public void Build(ReplyReceiveChannelSchema schema)
        {
            ReceiveMessageCache messageCache = this.persistenceFactorySelector
                .Select(schema)
                .CreateReceiveCache(PersistenceUseType.ReplyReceive, schema.Address);

            MessagePipelineBuilder.Build()
                .With(this.messageReceiver)
                .ToProcessor(new MessagePayloadCopier(this.serialiser))
                .ToProcessor(new BodyMessageFilter(schema.Address))
                .ToProcessor(new SequenceOriginApplier(messageCache))
                .ToProcessor(new MessageSendTimeRemover())
                .ToSimpleMessageRepeater(messageCache, this.systemTime, this.taskRepeater)
                .ToProcessor(new MessagePayloadCopier(this.serialiser))
                .ToProcessor(new ReceiveChannelMessageCacher(messageCache))
                .ToProcessor(new MessageAcknowledger(this.acknowledgementSender))
                .Queue()
                .ToResequencerIfSequenced(messageCache, schema)
                .ToConverter(new MessagePayloadUnpackager(this.serialiser))
                .ToProcessor(schema.UnitOfWorkRunnerCreator())
                .ToProcessor(new BatchUnpackager())
                .ToProcessors(schema.Hooks.ToArray())
                .ToEndPoint(this.messageHandlerRouter);

            Messenger.Send(new ChannelBuilt
            {
                CacheAddress = schema.Address,
                FromAddress = schema.ToAddress, 
                ToAddress = schema.Address, 
                UseType = PersistenceUseType.ReplyReceive
            });
        }
    }
}