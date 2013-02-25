using System.Diagnostics.Contracts;
using SystemDot.Messaging.Acknowledgement;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Batching;
using SystemDot.Messaging.Builders;
using SystemDot.Messaging.Caching;
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
    public class PointToPointReceiveChannelBuilder
    {
        readonly IMessageReceiver messageReceiver;
        readonly ISerialiser serialiser;
        readonly AcknowledgementSender acknowledgementSender;
        readonly MessageHandlerRouter messageHandlerRouter;
        readonly MessageCacheFactory messageCacheFactory;
        readonly ISystemTime systemTime;
        readonly ITaskRepeater taskRepeater;

        public PointToPointReceiveChannelBuilder(
            IMessageReceiver messageReceiver, 
            ISerialiser serialiser, 
            AcknowledgementSender acknowledgementSender, 
            MessageHandlerRouter messageHandlerRouter, 
            MessageCacheFactory messageCacheFactory, 
            ISystemTime systemTime, 
            ITaskRepeater taskRepeater)
        {
            Contract.Requires(messageReceiver != null);
            Contract.Requires(serialiser != null);
            Contract.Requires(acknowledgementSender != null);
            Contract.Requires(messageHandlerRouter != null);
            Contract.Requires(messageCacheFactory != null);
            Contract.Requires(systemTime != null);
            Contract.Requires(taskRepeater != null);

            this.messageReceiver = messageReceiver;
            this.serialiser = serialiser;
            this.acknowledgementSender = acknowledgementSender;
            this.messageHandlerRouter = messageHandlerRouter;
            this.messageCacheFactory = messageCacheFactory;
            this.systemTime = systemTime;
            this.taskRepeater = taskRepeater;
        }

        public void Build(ChannelSchema schema)
        {
            ReceiveMessageCache messageCache = this.messageCacheFactory.CreateReceiveCache(
                PersistenceUseType.PointToPointSend, 
                new EndpointAddress("Test", new ServerPath()));

            MessagePipelineBuilder.Build()
                .With(this.messageReceiver)
                .ToProcessor(new MessagePayloadCopier(this.serialiser))
                .ToProcessor(new SequenceOriginApplier(messageCache))
                .ToSimpleMessageRepeater(messageCache, this.systemTime, this.taskRepeater)
                .ToProcessor(new MessagePayloadCopier(this.serialiser))
                .ToProcessor(new ReceiveChannelMessageCacher(messageCache))
                .ToProcessor(new MessageAcknowledger(this.acknowledgementSender))
                .Queue()
                .ToResequencerIfSequenced(messageCache, schema)
                .ToConverter(new MessagePayloadUnpackager(this.serialiser))
                .ToProcessor(new BatchUnpackager())
                .ToEndPoint(this.messageHandlerRouter);
        }
    }
}