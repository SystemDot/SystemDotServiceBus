using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Aggregation;
using SystemDot.Messaging.Caching;
using SystemDot.Messaging.Filtering;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Pipelines;
using SystemDot.Messaging.Repeating;
using SystemDot.Messaging.Sequencing;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Transport;
using SystemDot.Parallelism;
using SystemDot.Serialisation;
using SystemDot.Storage.Changes;

namespace SystemDot.Messaging.PointToPoint.Builders
{
    public class PointToPointSendChannelBuilder
    {
        readonly IMessageSender messageSender;
        readonly ISerialiser serialiser;
        readonly InMemoryChangeStore inMemoryChangeStore;
        readonly ISystemTime systemTime;
        readonly ITaskRepeater taskRepeater;

        public PointToPointSendChannelBuilder(
            IMessageSender messageSender, 
            ISerialiser serialiser, 
            InMemoryChangeStore inMemoryChangeStore, 
            ISystemTime systemTime, 
            ITaskRepeater taskRepeater)
        {
            Contract.Requires(messageSender != null);
            Contract.Requires(serialiser != null);

            this.messageSender = messageSender;
            this.serialiser = serialiser;
            this.inMemoryChangeStore = inMemoryChangeStore;
            this.systemTime = systemTime;
            this.taskRepeater = taskRepeater;
        }

        public void Build(PointToPointSendChannelSchema schema)
        {
            Contract.Requires(schema != null);
            var messageCache = new MessageCache(this.inMemoryChangeStore, schema.FromAddress, PersistenceUseType.PointToPointSend);

            MessagePipelineBuilder.Build()
                .WithBusSendTo(new MessageFilter(new PassThroughMessageFilterStategy()))
                .ToProcessor(new AggregatePackager())
                .ToConverter(new MessagePayloadPackager(this.serialiser))
                .ToProcessor(new Sequencer(messageCache))
                .ToProcessor(new MessageAddresser(schema.FromAddress, schema.RecieverAddress))
                .ToMessageRepeater(messageCache, this.systemTime, this.taskRepeater, schema.RepeatStrategy)
                .ToProcessor(new MessagePayloadCopier(this.serialiser))
                .ToProcessor(new SendChannelMessageCacher(messageCache))
                .ToProcessor(new PersistenceSourceRecorder())
                .Queue()
                .ToEndPoint(this.messageSender);
        }
    }
}