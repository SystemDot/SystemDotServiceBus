using System.Diagnostics.Contracts;
using SystemDot.Messaging.Acknowledgement;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Batching;
using SystemDot.Messaging.Builders;
using SystemDot.Messaging.Caching;
using SystemDot.Messaging.Expiry;
using SystemDot.Messaging.Filtering;
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
    class PointToPointSendChannelBuilder
    {
        readonly IMessageSender messageSender;
        readonly ISerialiser serialiser;
        readonly ISystemTime systemTime;
        readonly ITaskRepeater taskRepeater;
        readonly PersistenceFactorySelector persistenceFactorySelector;
        readonly MessageAcknowledgementHandler acknowledgementHandler;

        public PointToPointSendChannelBuilder(
            IMessageSender messageSender, 
            ISerialiser serialiser, 
            ISystemTime systemTime, 
            ITaskRepeater taskRepeater, 
            PersistenceFactorySelector persistenceFactorySelector, 
            MessageAcknowledgementHandler acknowledgementHandler)
        {
            Contract.Requires(messageSender != null);
            Contract.Requires(serialiser != null);
            Contract.Requires(systemTime != null);
            Contract.Requires(taskRepeater != null);
            Contract.Requires(persistenceFactorySelector != null);
            Contract.Requires(acknowledgementHandler != null);

            this.messageSender = messageSender;
            this.serialiser = serialiser;
            this.systemTime = systemTime;
            this.taskRepeater = taskRepeater;
            this.persistenceFactorySelector = persistenceFactorySelector;
            this.acknowledgementHandler = acknowledgementHandler;
        }

        public void Build(PointToPointSendChannelSchema schema)
        {
            Contract.Requires(schema != null);

            SendMessageCache cache = this.persistenceFactorySelector
                .Select(schema)
                .CreateSendCache(PersistenceUseType.PointToPointSend, schema.FromAddress);

            this.acknowledgementHandler.RegisterCache(cache);

            MessagePipelineBuilder.Build()
                .WithBusSendTo(new MessageFilter(new PassThroughMessageFilterStategy()))
                .ToProcessor(new BatchPackager())
                .ToConverter(new MessagePayloadPackager(this.serialiser))
                .ToProcessor(new Sequencer(cache))
                .ToProcessor(new MessageAddresser(schema.FromAddress, schema.ReceiverAddress))
                .ToMessageRepeater(cache, this.systemTime, this.taskRepeater, schema.RepeatStrategy)
                .ToProcessor(new MessagePayloadCopier(this.serialiser))
                .ToProcessor(new SendChannelMessageCacher(cache))
                .ToProcessor(new SequenceOriginRecorder(cache))
                .ToProcessor(new PersistenceSourceRecorder())
                .Queue()
                .ToProcessor(new MessageExpirer(schema.ExpiryStrategy, cache))
                .ToEndPoint(this.messageSender);

            Messenger.Send(new PointToPointSendChannelBuilt
            {
                SenderAddress = schema.FromAddress,
                ReceiverAddress = schema.ReceiverAddress
            });
        }
    }
}